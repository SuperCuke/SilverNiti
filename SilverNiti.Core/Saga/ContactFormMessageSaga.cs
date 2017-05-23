using System.Net.Mail;
using Ozzy;
using Ozzy.DomainModel;
using Ozzy.Server;
using SilverNiti.Core.DomainEvents;
using Typesafe.Mailgun;

namespace SilverNiti.Core.Saga
{

    public class ContactFormMessageSaga : SagaBase<ContactFormMessageSaga.Data>,
        IHandleEvent<ContactFormMessageRecieved>,
        IHandleEvent<ContactFormMessageSaga.SendGreetingEmail>,
        IHandleEvent<ContactFormMessageSaga.SendNotificationToAdministrator>
    {
        private IMailgunClient _mailgun;

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
            public string To { get; protected set; }
            public string From { get; protected set; }
            public string Message { get; protected set; }
            public string Subject { get; protected set; }

            public SendGreetingEmail(ContactFormMessageSaga saga) : base(saga)
            {
                Guard.ArgumentNotNull(saga, nameof(saga));
                To = saga.State.From;
                From = "admin@silverniti.ru";
                Message = "Thank for you message. We will contact yoyu back soon";
                Subject = "Your message is imporant for us!";
            }

            //Serializable constructor
            protected SendGreetingEmail()
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

        public ContactFormMessageSaga(IMailgunClient mailgun)
        {
            _mailgun = mailgun;
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
            var mail = new MailMessage(message.From, message.To, message.Subject, message.Message);
            _mailgun.SendMail(mail);
            State.GreetingEmailSent = true;
            CheckSagaComplete();
            return false;
        }

        public bool Handle(SendNotificationToAdministrator message)
        {
            //var command = new EmailMailCommand()
            //{
            //    To = "inbox@ozzy.com",
            //    From = State.From,
            //    Message = State.Message
            //};        
            //_mediator.Send(command);
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
