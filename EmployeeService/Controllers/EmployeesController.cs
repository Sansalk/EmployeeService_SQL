using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDAL;


namespace EmployeeService.Controllers
{
    public class EmployeesController : ApiController
    {
        public HttpResponseMessage  Get(string gender ="All")
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {

                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(x=>x.Gender.ToLower()=="male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(x => x.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");
                }

            }
        }

        [HttpGet]
        public HttpResponseMessage LoadEmp(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                try
                {

                    var entity = entities.Employees.FirstOrDefault(x => x.ID == id);

                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            using (EmployeeDBEntities enities = new EmployeeDBEntities())
            {
                try
                {
                    enities.Employees.Add(employee);
                    enities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);

                    message.Headers.Location = new Uri(Request.RequestUri +"/"+ employee.ID.ToString());
                    return message;
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }


        public HttpResponseMessage Put(int id,[FromBody] Employee employee)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                try
                {
                    var Entitiy = entities.Employees.FirstOrDefault(x => x.ID == id);

                    if (Entitiy != null)
                    {
                        Entitiy.FirstName = employee.FirstName;
                        Entitiy.LastName = employee.LastName;
                        Entitiy.Gender = employee.Gender;
                        Entitiy.Salary = employee.Salary;
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, "Updated");

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Updated");

                    }

                }
                catch(Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex);

                }
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            using (EmployeeDBEntities enities = new EmployeeDBEntities())
            {
                try
                {
                    var enitiy =enities.Employees.FirstOrDefault(x => x.ID == id);

                    if (enitiy == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");

                    }
                    else
                    {
                        enities.Employees.Remove(enitiy);
                        enities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
                    }
                   
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }
    }
}
