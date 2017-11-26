using System;
using System.IO;

namespace TestNinja.Mocking
{
    public interface IStatementGenerator
    {
        string SaveStatement(int housekeeperOid, string housekeeperName, DateTime statementDate);
    }

    public class StatementGenerator : IStatementGenerator
    {
        public string SaveStatement(int housekeeperOid, string housekeeperName, DateTime statementDate)
        {
            var report = new HousekeeperStatementReport(housekeeperOid, statementDate);

            if (!report.HasData)
                return string.Empty;

            report.CreateDocument();

            var filename = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Sandpiper Statement {statementDate:yyyy-MM} {housekeeperOid}.pdf");

            report.ExportToPdf(filename);

            return filename;
        }
    }

    public class HousekeeperStatementReport
    {
        public HousekeeperStatementReport(int housekeeperOid, DateTime statementDate)
        {
            throw new NotImplementedException();
        }

        public bool HasData { get; set; }

        public void CreateDocument()
        {
            throw new NotImplementedException();
        }

        public void ExportToPdf(string filename)
        {
            throw new NotImplementedException();
        }
    }
}