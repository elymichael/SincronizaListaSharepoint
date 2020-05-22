namespace SincronizaListaSharepoint
{
    using Microsoft.SharePoint.Client;
    using System.Configuration;
    using System.Data;
    using System.Security;

    public class SharepointInteraction
    {
        /// <summary>
        /// Sharepoint client context.
        /// </summary>
        private ClientContext context;

        /// <summary>
        /// Default constructor that receive sharepoint site.
        /// </summary>
        /// <param name="sharepointsite">site url</param>
        public SharepointInteraction(string sharepointsite, string username, string password)
        {
            SecureString passWord = new SecureString();

            foreach (char c in password.ToCharArray()) passWord.AppendChar(c);

            context = new ClientContext(sharepointsite);
            context.Credentials = new SharePointOnlineCredentials(username, passWord);
        }

        /// <summary>
        /// Delete all the items inside a sharepoint list.
        /// </summary>
        public void deleteItem(string listname)
        {
            List sharepointList = context.Web.Lists.GetByTitle(listname);

            var camlQuery = new CamlQuery();
            ListItemCollection collection = sharepointList.GetItems(camlQuery);
            context.Load(collection,
            eachItem => eachItem.Include(
                item => item,
                item => item["ID"]));
            context.ExecuteQuery();

            int totalListItems = collection.Count;

            for (var i = totalListItems - 1; i > -1; i--)
            {
                collection[i].DeleteObject();
            }
            context.ExecuteQuery();
        }

        /// <summary>
        /// Accept a datarow with the fields to add to the sharepoint's list.
        /// </summary>
        /// <param name="listname">list name</param>
        /// <param name="dr">data row</param>
        public void AddItem(string listname, DataRow dr, UserInformation user)
        {
            List sharepointList = context.Web.Lists.GetByTitle(listname);

            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
            ListItem newItem = sharepointList.AddItem(itemCreateInfo);
            newItem["Title"] = dr["Title"].ToString();
            newItem["JobTitle"] = dr["JobTitle"].ToString();
            newItem["Birthday"] = dr["BirthDay"].ToString();
            newItem["email"] = dr["Email"].ToString();
            newItem["userAADGUID"] = user.UUID;
            newItem["phone"] = user.Phone;
            newItem["department"] = user.Department;
            newItem.Update();
        }

        public void ExecuteQuery()
        {
            context.ExecuteQuery();
        }
    }
}
