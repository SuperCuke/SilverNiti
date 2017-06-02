using System.Net.Mail;
using Ozzy;
using Ozzy.DomainModel;
using Ozzy.Server;
using SilverNiti.Core.DomainEvents;
using Typesafe.Mailgun;
using Microsoft.Extensions.Options;
using SilverNiti.Core.Domain;

namespace SilverNiti.Core.Saga
{

    public class ContactFormMessageSaga : SagaBase<ContactFormMessageSaga.Data>,
        IHandleEvent<ContactFormMessageRecieved>,
        IHandleEvent<ContactFormMessageSaga.SendGreetingEmail>,
        IHandleEvent<ContactFormMessageSaga.SendNotificationToAdministrator>
    {
        //  private IMailgunClient _mailgun;

        public class Data
        {
            public string Message { get; set; }
            public string From { get; set; }
            public int MessageId { get; set; }
            public bool GreetingEmailSent { get; set; }
            public bool AdminEmailSent { get; set; }
            public bool IsComplete { get; set; }
        }

        public class SendGreetingEmail : SagaCommand
        {
            public string To { get; set; }
            public string From { get; set; }
            public string Message { get; set; }
            public string Subject { get; set; }

            public SendGreetingEmail(ContactFormMessageSaga saga) : base(saga)
            {
                To = saga.State.From;
                From = saga.MailgunConfiguration.FromEmail;
                Message = saga.State.Message;
            }

            //Serializable constructor
            public SendGreetingEmail()
            {
            }
        }
        public class SendNotificationToAdministrator : SagaCommand
        {
            public SendNotificationToAdministrator(ContactFormMessageSaga saga) : base(saga)
            {
            }

            public SendNotificationToAdministrator()
            {
            }
        }

        //private IMediator _mediator;

        public IMailgunClient MailgunClient { get; set; }
        public MailgunConfiguration MailgunConfiguration { get; set; }
        public ContactFormMessageSaga(IMailgunClient mailgunClient, IOptions<MailgunConfiguration> mailgunOptions)
        {
            MailgunClient = mailgunClient;
            MailgunConfiguration = mailgunOptions.Value;
        }

        public bool Handle(ContactFormMessageRecieved message)
        {
            State.Message = message.Message;
            State.From = message.Email;
            State.MessageId = message.MessageId;

            SendSagaCommand(new SendGreetingEmail(this));
            SendSagaCommand(new SendNotificationToAdministrator(this));
            return false;
        }

        public bool Handle(SendGreetingEmail message)
        {
            MailgunClient.SendMail(new MailMessage(MailgunConfiguration.FromEmail, State.From)
            {
                Subject = "Your message is imporant for us!",
                Body = "Thank for you message. We will contact yoyu back soon"
            });

            State.GreetingEmailSent = true;
            CheckSagaComplete();
            return false;
        }

        public bool Handle(SendNotificationToAdministrator message)
        {
            MailgunClient.SendMail(new MailMessage(MailgunConfiguration.FromEmail, State.From)
            {
                Subject = "New email from customer",
                Body = $"New message from customer /n" +
                $"Email: {State.From} /n" +
                $"Message: {State.Message} /n"
            });

            State.AdminEmailSent = true;
            CheckSagaComplete();
            return false;
        }

        public void CheckSagaComplete()
        {
            if (State.GreetingEmailSent && State.AdminEmailSent)
            {
                State.IsComplete = true;
            }
        }
    }
}
