using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectCompare;
using System.Linq.Expressions;

namespace ConsoleApplication1
{
    class Program
    {
        public static E10ProductionDataContext _e10Prod = new E10ProductionDataContext();
        public static E10ProductionDataContext _e10Dev = new E10ProductionDataContext("Data Source=Lv-sql-01;Initial Catalog=E10DEV_THREE;Integrated Security=True");
        static void Main(string[] args)
        {
            string company = "FUS";
            int sourceOrderNum = 101071;
            int targetOrderNum = 100994;
            string sourceProjectId = "101071";
            string targetProjectId = "100994";
            CompareCustomer("FUS", 4378, 4356);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            CompareOrderHed(company, sourceOrderNum, targetOrderNum);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            CompareProject(company, sourceProjectId, targetProjectId);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            CompareProjectCst(company, sourceProjectId, targetProjectId);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();            
        }
        public static void CompareCustomer(string company, int sourceCustNum, int targetCustNum)
        {
            CompareDbObjects<Customer> customerCompare = new CompareDbObjects<Customer>(_e10Prod.Connection, _e10Dev.Connection);
            List<string> exempt = new List<string> { "Customer_UD", "SysRowID", "SysRevID" };
            List<string> subObjects = new List<string> { "Customer_UD" };
            Expression<Func<Customer, bool>> sourceExpr = p => p.Company == company && p.CustNum == sourceCustNum;
            Expression<Func<Customer, bool>> targetExpr = p => p.Company == company && p.CustNum == targetCustNum;
            var data = customerCompare.CompareOneToOne(exempt, subObjects, sourceExpr, targetExpr, Console.Out);
        }
        public static void CompareOrderHed(string company, int sourceOrderNum, int targetOrderNum)
        {
            CompareDbObjects<OrderHed> ohCompare = new CompareDbObjects<OrderHed>(_e10Prod.Connection, _e10Dev.Connection);
            List<string> exempt = new List<string> { "OrderHed_UD", "SysRowID", "SysRevID" };
            List<string> subObjects = new List<string> { "OrderHed_UD" };
            Expression<Func<OrderHed, bool>> sourceExpr = p => p.OrderNum == sourceOrderNum;
            Expression<Func<OrderHed, bool>> targetExpr = p => p.OrderNum == targetOrderNum;
            var data = ohCompare.CompareOneToOne(exempt, subObjects, sourceExpr, targetExpr, Console.Out);
        }
        public static void CompareProject(string company, string sourceProjectId, string targetProjectId)
        {
            CompareDbObjects<Project> projectCompare = new CompareDbObjects<Project>(_e10Prod.Connection, _e10Dev.Connection);
            List<string> exempt = new List<string> { "Project_UD", "SysRowID", "SysRevID" };
            List<string> subObjects = new List<string> { "Project_UD" };
            Expression<Func<Project, bool>> sourceExpr = p => p.ProjectID == sourceProjectId;
            Expression<Func<Project, bool>> targetExpr = p => p.ProjectID == targetProjectId;
            var data = projectCompare.CompareOneToOne(exempt, subObjects, sourceExpr, targetExpr, Console.Out);
        }
        public static void CompareProjectCst(string company, string sourceProjectId, string targetProjectId)
        {
            CompareDbObjects<ProjectCst> projectCstCompare = new CompareDbObjects<ProjectCst>(_e10Prod.Connection, _e10Dev.Connection);
            List<string> exempt = new List<string> { "SysRowID", "SysRevID" };
            List<string> subObjects = new List<string>();
            Expression<Func<ProjectCst, bool>> sourceExpr = p => p.ProjectID == sourceProjectId;
            Expression<Func<ProjectCst, bool>> targetExpr = p => p.ProjectID == targetProjectId;
            var data = projectCstCompare.CompareOneToOne(exempt, subObjects, sourceExpr, targetExpr, Console.Out);
        }
    }
}
