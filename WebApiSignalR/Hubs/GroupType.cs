namespace WebApiSignalR.Hubs
{
    public class GroupType
    {
        public string Group_Name { get; set; }
        public int UserCont { get; set; }
        private double myVar;

        public string Price
        {
            get { return myVar.ToString(); }
            set { myVar = Convert.ToDouble(value); }
        }

        public GroupType(string Group_Name, int UserCont,double price)
        {
            this.Group_Name = Group_Name;
            this.UserCont = UserCont;
            myVar = price;
        }

    }
}
