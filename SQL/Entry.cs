using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.SQL
{
    class Entry
    {
        //Перечисление (enum), содержащее в себе варианты результатов сравнения двух позиций, в том числе элемент Error, который будет возвращен в случае,
        //если во время чтения данных из базы возникнет ошибка на первом или втором этапе
        public enum CheckResult 
        { 
            Unique, 
            NonChanged,
            Cancelled, 
            M_purchase,
            M_publishYear,
            M_tot,
            M_OKPD,
            M_KVR,
            M_KBK,
            Error
        };

        //Метод, сравнивающий две строки. Заменяет String.Compare, поскольку тот не умеет автоматически обрезать лишние пробелы в начале и конце строки,
        //а это необходимо делать, потому что некоторые поля в базе не оптимизированы и хранят больше знаков, чем нужно (вероятно, этой проблемой стоит заняться позже).
        //Возвращает true, если две строки после Trim равны или обе являются IsNullOrEmpty.
        public static bool CompareTwoStrings(string s1, string s2)
        {
            if (!String.IsNullOrEmpty(s1)) s1 = s1.Trim(' ');
            if (!String.IsNullOrEmpty(s2)) s2 = s2.Trim(' ');

            if (String.IsNullOrEmpty(s1) && !String.IsNullOrEmpty(s2)) return false;
            else if (String.IsNullOrEmpty(s2) && !String.IsNullOrEmpty(s1)) return false;
            else if (String.IsNullOrEmpty(s1) && String.IsNullOrEmpty(s2)) return true;
            else if (!String.IsNullOrEmpty(s2) && !String.IsNullOrEmpty(s1))
                {
                if (s1 == s2) return true;
                else return false;
                }

            return false;
        }

        //Главный метод класса, возвращающий результат поиска и сравнения заданной позиции с существующей позицией в базе, работа выполняется в два этапа:
        //
        //I этап: поиск заданной позиции в базе, в качестве ключа используется уникальный номер позиции. Если позиции с таким номером не существует в базе,
        //возвращается CheckResult.Unique (позиция является уникальной). Если позиция с таким номером существует в базе, выполняется этап II.
        //
        //II этап: сравнение заданной позиции с найденной по ключу позицией в базе. Если никаких изменений не найдено, возвращается CheckResult.NonChanged.
        //Если позиция отличается от найденной в базе, по очереди проверяется каждое поле из перечисленных. (Позже стоит изменить метод так, чтобы он возвращал массив
        //CheckResult на тот случай, если в позиции изменилось более чем одно поле)
        //
        //Параметр databaseName отмечен как out, по умолчанию является null, но если позиция не является уникальной, в него заносится значение поля DatabaseName
        //найденной в базе позиции, с которой осуществляется сравнение, чтобы в дальнейшем использовать его в запросе UPDATE
        public static CheckResult GetNumberOfEntries(XML.PurchasePlanPosition.Position position, string positionNumber, DateTime dateTime, out string databaseName)
        {
            using (SqlConnection connection = new SqlConnection(Builder.Line().ConnectionString))
            {
                databaseName = null;

                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(Query.GetCountOfRowsWithIdentifier(), connection))
                    {
                        int count = 0;

                        command.Parameters.AddWithValue("@positionNumber", positionNumber);

                        count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            using (SqlCommand _command = new SqlCommand(Query.GetRoWithIdentifier(), connection))
                            {
                                _command.Parameters.AddWithValue("@positionNumber", positionNumber);

                                using (SqlDataReader reader = _command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        int pubYear = reader.GetInt32(0);
                                        double tot = reader.GetDouble(1);
                                        bool posCanc = reader.GetBoolean(2);
                                        string OKPD = reader[3] as string; //reader.GetString(3);
                                        DateTime modified = reader.GetDateTime(4);
                                        DateTime firstEntry = reader.GetDateTime(5);
                                        databaseName = reader[6] as string;
                                        string purchaseObjectInfo = reader[8] as string;
                                        string KVRCode = reader[9] as string;
                                        string KBK = reader[10] as string;

                                        if (position.publishYear != pubYear ||
                                            position.total != tot ||
                                            position.positionCanceled != posCanc ||
                                            !CompareTwoStrings(position.OKPDCode.GetText(), OKPD) ||//position.OKPDCode.GetText().Trim(' ') != OKPD.Trim(' ') ||
                                            !CompareTwoStrings(position.purchaseObjectInfo.GetText(), purchaseObjectInfo) ||//position.purchaseObjectInfo.GetText().Trim(' ') != purchaseObjectInfo.Trim(' ') ||
                                            !CompareTwoStrings(position.KVRcode.GetText(), KVRCode) ||//position.KVRcode.GetText().Trim(' ') != KVRCode.Trim(' ') ||
                                            !CompareTwoStrings(position.KBK.GetText(), KBK))//position.KBK.GetText().Trim(' ') != KBK.Trim(' '))
                                        {
                                            if (dateTime > modified)
                                            {
                                                if (position.positionCanceled) return CheckResult.Cancelled;
                                                else if (position.publishYear != pubYear) return CheckResult.M_publishYear;
                                                else if (position.total != tot) return CheckResult.M_tot;
                                                else if (position.OKPDCode.GetText() != OKPD) return CheckResult.M_OKPD;
                                                else if (position.purchaseObjectInfo.GetText() != purchaseObjectInfo) return CheckResult.M_purchase;
                                                else if (position.KVRcode.GetText() != KVRCode) return CheckResult.M_KVR;
                                                else if (position.KBK.GetText() != KBK) return CheckResult.M_KBK;
                                            }
                                            //else return CheckResult.NotLastModified;
                                        }
                                        else return CheckResult.NonChanged;
                                    }
                                    return CheckResult.Error;
                                }
                            }
                            //return CheckResult.shared;
                        }
                        else return CheckResult.Unique;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Date: {dateTime} / Database: {databaseName}");
                    Console.WriteLine(e);

                    return CheckResult.Error;
                }
            }
        }
    }
}
