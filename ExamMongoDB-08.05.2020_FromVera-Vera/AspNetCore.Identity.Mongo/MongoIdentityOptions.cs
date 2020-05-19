﻿namespace AspNetCore.Identity.Mongo
{
	public class MongoIdentityOptions
	{
		public string ConnectionString { get; set; } //= "mongodb://localhost/default";
        
	    public string UsersCollection { get; set; } = "student";
		
	    public string RolesCollection { get; set; } = "Roles";

	    //public bool UseDefaultIdentity { get; set; } = true;
	}
}