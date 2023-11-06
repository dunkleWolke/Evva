using Evva.Models;
using Evva.XMLSerializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Evva.DeserializedUserNamespace
{
    public static class DeserializedUser
    {
        public static User deserializedUser;

        static DeserializedUser()
        {
            try
            {
                deserializedUser = XmlSerializeWrapper<User>.Deserialize("../lastUser.xml", FileMode.Open);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
}
    }
}
