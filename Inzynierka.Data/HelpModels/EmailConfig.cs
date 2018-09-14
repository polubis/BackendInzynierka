using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.HelpModels
{
    public class EmailConfig
    {
        public readonly string registerMessage = String.Format("@Dziękujemy za utworzenie konta w naszym serwisie.Poniżej przesyłamy " +
           "link aktywacyjny do twojego konta." + " Jeżeli to nie ty je utworzyłeś to poprostu zignoruj te wiadomość");

        public readonly string changeEmailMessage = String.Format("@Witaj. Aby zmienić adres mailowy wystarczy, że klikniesz w poniższy link" +
            " aktywacyjny. Jeżeli to nie ty go utworzyłeś to poprostu zignoruj tę wiadomość");

        public readonly string registerLink = "http://localhost:3000/register/activate/";
        public readonly string changeEmailLink = "http://localhost:3000/changemail/activate/";
    }
}
