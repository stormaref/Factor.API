namespace Factor.Models.ResponseModels
{
    public class TokenResponseModel
    {
        public string Phone { get; set; }
        public string Token { get; set; }

        public TokenResponseModel(string phone, string token)
        {
            Phone = phone;
            Token = token;
        }
    }
}
