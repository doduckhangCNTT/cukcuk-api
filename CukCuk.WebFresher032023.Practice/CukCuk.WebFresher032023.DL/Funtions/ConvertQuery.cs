using CukCuk.WebFresher032023.DL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.DL.Funtions
{
    public class ConvertQuery
    {
        /// <summary>
        /// - Thực hiện tạo câu query
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static string BuildSqlQuery(List<EntityItemFilter> attributes)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("SELECT * FROM Food WHERE ");

            for (int i = 0; i < attributes.Count; i++)
            {
                EntityItemFilter attribute = attributes[i];
                // Lấy câu truy vấn điều kiện
                string condition = GetCondition(attribute);

                // Lí do i > 0 là để làm cho câu lệnh điều kiện "đầu tiên" không có "addition"
                if (i > 0)
                {
                    string addition = attribute.Addition ?? "";
                    if (!string.IsNullOrEmpty(addition))
                    {
                        // Thực hiện nối câu lệnh điều kiện sql
                        sb.Append($" {addition} ");
                    }
                }
                // Nối sql điều kiện
                sb.Append(condition);
            }

            return sb.ToString();
        }

        /// <summary>
        /// - Thực hiện tạo câu lên truy vấn "điều kiện" dựa trên toán tử
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetCondition(EntityItemFilter attribute)
        {
            string property = attribute.Property;
            string operatorValue = GetSqlOperator(attribute.Operator);
            string type = attribute.Type;
            object value = attribute.Value;

            if (type.ToLower() == "string")
            {
                string stringValue = value.ToString();
                switch (operatorValue.ToUpper())
                {
                    case "STARTWITH":
                        return $"{property} LIKE '{stringValue}%'";

                    case "ENDWITH":
                        return $"{property} LIKE '%{stringValue}'";

                    case "LIKE":
                        return $"{property} LIKE '%{stringValue}%'";

                    case "NOTLIKE":
                        return $"{property} NOT LIKE '%{stringValue}%'";
                    case "=":
                        return $"{property} = '{stringValue}'";
                    case "IN":
                        return $"{property} IN {stringValue}";
                    default:
                        throw new ArgumentException("Invalid operator for string type");
                }
            }
            else
            {
                return $"{property} {operatorValue} {value}";
            }
        }

        /// <summary>
        /// - Thực hiện lấy toán tử tương ứng với giá trị toán tử truyền vào 
        /// </summary>
        /// <param name="operatorValue">Gía trị toán tử</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// Author: DDKhang (26/6/2023)
        public static string GetSqlOperator(string operatorValue)
        {
            switch (operatorValue.ToUpper())
            {
                case "=":
                case "EQUAL":
                    return "=";

                case ">":
                case "GREATER":
                    return ">";

                case ">=":
                case "GREATEREQUAL":
                    return ">=";

                case "<":
                case "LESS":
                    return "<";

                case "<=":
                case "LESSEQUAL":
                    return "<=";

                case "LIKE":
                    return "LIKE";

                case "STARTWITH":
                    return "STARTWITH";

                case "ENDWITH":
                    return "ENDWITH";

                case "NOTLIKE":
                    return "NOTLIKE";
                case "IN":
                    return "IN";
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }
    }
}
