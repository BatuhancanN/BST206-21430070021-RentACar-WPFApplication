using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarApplication
{
    public class SqlServerBaglanti
    {
        private static string connectionString;

        public SqlServerBaglanti()
        {
            // Sql Server Management Studio 18 ve üstü sürümler geçerlidir.
            // Sql Server üzerinde bir kullanıcı oluşturun; bu kullanıcıya yazma ve okuma iznini, proje içerisindeki veri tabanı için verin.
            // İster aşağıdaki bilgilerle aynı yapın, ister farklı yapıp aşağıya bilgileri girin. Sorunsuz şekilde çalışacaktır.
            connectionString = "Data Source=localhost;Initial Catalog=RentACarDB;User ID=RentACarLogin;Password=batuhan0633";
        }

        public string baglanti()
        {
            return connectionString;
        }
    }
}
