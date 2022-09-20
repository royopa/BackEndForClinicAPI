namespace BackEndForClinicAPI.Models
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() { UserName = "jason_admin", EmailAddress = "jason.admin@email.com", GivenName = "Json", Password = "MyPassW0rd", Surname = "Bryant", Role = "Administrator"},
            new UserModel() { UserName = "elyse_seller", EmailAddress = "elyse.seller@email.com", GivenName = "Elyse", Password = "MyPassW0rd", Surname = "Mark", Role = "Seller"}
        };
    }
}
