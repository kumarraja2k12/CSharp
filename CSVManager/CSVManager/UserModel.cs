using System;
namespace CSVManager
{
    public class UserModel
    {
        /*public UserModel(string name, string type, string phone, string state)
        {
            Name = name;
            Type = type;
            Phone = phone;
            State = state;
        }*/

        public string Name { get; set; }
        public string Type { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }

        public bool Equals(UserModel model)
        {
            return Name.Equals(model.Name) &&
                Type.Equals(model.Type) &&
                Phone.Equals(model.Phone);
        }
    }
}
