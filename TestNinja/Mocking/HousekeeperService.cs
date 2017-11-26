using System;
using System.IO;

namespace TestNinja.Mocking
{
    public class HousekeeperService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStatementGenerator _statementGenerator;
        private readonly IEmailSender _emailSender;
        private readonly IXtraMessageBox _messageBox;

        public HousekeeperService(
            IUnitOfWork unitOfWork, 
            IStatementGenerator statementGenerator,
            IEmailSender emailSender,
            IXtraMessageBox messageBox)
        {
            _statementGenerator = statementGenerator;
            _emailSender = emailSender;
            _messageBox = messageBox;
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public bool SendStatementEmails(DateTime statementDate)
        {
            var housekeepers = _unitOfWork.Query<Housekeeper>();

            foreach (var housekeeper in housekeepers)
            {
                if (string.IsNullOrWhiteSpace(housekeeper.Email))
                    continue;

                var statementFilename = _statementGenerator.SaveStatement(housekeeper.Oid, 
                    housekeeper.FullName, 
                    statementDate);

                if (string.IsNullOrWhiteSpace(statementFilename))
                    continue;

                var emailAddress = housekeeper.Email;
                var emailBody = housekeeper.StatementEmailBody;

                try
                {
                    _emailSender.EmailFile(emailAddress, emailBody, statementFilename,
                        $"Sandpiper Statement {statementDate:yyyy-MM} {housekeeper.Oid}");
                }
                catch (Exception e)
                {
                    _messageBox.Show(e.Message, $"Email failure: {emailAddress}", MessageBoxButtons.OK);
                }
            }

            return true;
        }
    }

    public enum MessageBoxButtons
    {
        OK
    }

    public interface IXtraMessageBox
    {
        void Show(string s, string housekeeperStatements, MessageBoxButtons ok);
    }

    public class XtraMessageBox : IXtraMessageBox
    {
        public void Show(string s, string housekeeperStatements, MessageBoxButtons ok)
        { }
    }

    public class MainFrom
    {
        public bool HousekeeperStatementsSending { get; set; }
    }

    public class DateFrom
    {
        public DateFrom(string statementDate, object endOfLastMonth)
        {
        }
    }

    public class Housekeeper
    {
        public string Email { get; set; }
        public string StatementEmailBody { get; set; }
        public int Oid { get; set; }
        public string FullName { get; set; }
    }
}