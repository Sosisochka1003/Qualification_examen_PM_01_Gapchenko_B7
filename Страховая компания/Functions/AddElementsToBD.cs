using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Страховая_компания.DataBase;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Страховая_компания.Functions
{
    class AddElementsToBD
    {
        public static string AddClient(string last_name, string name, string patronymic, string gender, DateTime date_of_birth, string passport_series, string passport_number)
        {
            try
            {
                using var ctx = new ExamenContext();
                Clients clients = new Clients
                {
                    last_name = last_name,
                    name = name,
                    patronymic = patronymic,
                    gender = gender,
                    date_of_birth = DateOnly.FromDateTime(date_of_birth),
                    passport_series = Convert.ToInt32(passport_series),
                    passport_number = Convert.ToInt32(passport_number),
                };
                ctx.client.Add(clients);
                ctx.SaveChanges();
                return "Добавленно";
            }
            catch (Exception ex)
            {
                return "Ошибка заполнения данных";
            }   
        }

        public static string AddEmp(string branch, string last_name, string name, string patronymic)
        {
            using var ctx = new ExamenContext();
            try
            {
                Employees employee = new Employees
                {
                    branch = Convert.ToInt32(branch),
                    last_name = last_name,
                    name = name,
                    patronymic = patronymic
                };
                ctx.employee.Add(employee);
                ctx.SaveChanges();
                return "Добавленно";
            }
            catch (Exception ex)
            {
                return "Ошибка заполнения данных";
            }      
        }

        public static string AddInsurancePayment(DateTime date, string id_treaty, string payout_amount)
        {
            using var ctx = new ExamenContext();
            try
            {
                Treaty treaty = ctx.treaty.FirstOrDefault(t => t.tid == Convert.ToInt32(id_treaty));
                if (treaty == null) { return "Неверный номер договора"; }
                InsurancePayments insurance = new InsurancePayments
                {
                    date = DateOnly.FromDateTime(date),
                    id_treaty = Convert.ToInt32(id_treaty),
                    treaty = treaty,
                    payout_amount = Convert.ToInt32(payout_amount)
                };
                ctx.insurancepayment.Add(insurance);
                ctx.SaveChanges();
                return "Добавленно";
            }
            catch (Exception ex)
            {
                return "Ошибка заполнения данных";
            }
        }

        public static string AddObjectOfInsurance(string name)
        {
            using var ctx = new ExamenContext();
            try
            {
                ObjectsOfInsurance objects = new ObjectsOfInsurance
                {
                    name = name
                };
                ctx.objectofinsurance.Add(objects);
                ctx.SaveChanges();
                return "Добавленно";
            }
            catch (Exception ex)
            {
                return "Ошибка заполнения данных";
            }
        }

        public static string AddTreaty(DateTime date_of_conclussion, string id_client, string id_emp, string id_object, string insurance_payment)
        {
            using var ctx = new ExamenContext();
            try
            {
                Clients clients = ctx.client.FirstOrDefault(c => c.cid == Convert.ToInt32(id_client));
                Employees employee = ctx.employee.FirstOrDefault(e => e.eid == Convert.ToInt32(id_emp));
                ObjectsOfInsurance objects = ctx.objectofinsurance.FirstOrDefault(o => o.ooiid == Convert.ToInt32(id_object));
                if (clients == null) { return "Неверный клиент"; }
                if (employee == null) { return "Неверный сотрудник"; }
                if (objects == null) { return "Неверный обьект страхования"; }
                Treaty treaty = new Treaty
                {
                    date_of_conclusion = DateOnly.FromDateTime(date_of_conclussion),
                    id_client = Convert.ToInt32(id_client),
                    clients = clients,
                    id_emp = Convert.ToInt32(id_emp),
                    employees = employee,
                    id_object = Convert.ToInt32(id_object),
                    objects = objects,
                    insurance_payment = Convert.ToInt32(insurance_payment)
                };
                ctx.treaty.Add(treaty);
                ctx.SaveChanges();
                return "Добавленно";
            }
            catch (Exception ex)
            {
                return "Ошибка заполнения данных";
            }
        }
    }
}
