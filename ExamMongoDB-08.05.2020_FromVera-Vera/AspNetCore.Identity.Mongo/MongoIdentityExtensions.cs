﻿using System;
using AspNetCore.Identity.Mongo.Model;
using AspNetCore.Identity.Mongo.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace AspNetCore.Identity.Mongo
{
	public static class MongoIdentityExtensions
	{
	    


	    public static IdentityBuilder AddIdentityMongoDbProvider<TUser>(this IServiceCollection services,
	        Action<IdentityOptions> setupIdentityAction, Action<MongoIdentityOptions> setupDatabaseAction) where TUser : MongoUser
	    {
	        return AddIdentityMongoDbProvider<TUser, MongoRole>(services, setupIdentityAction, setupDatabaseAction);
        }

	    public static IdentityBuilder AddIdentityMongoDbProvider<TUser, TRole>(this IServiceCollection services,
	        Action<IdentityOptions> setupIdentityAction, Action<MongoIdentityOptions> setupDatabaseAction) where TUser : MongoUser
	        where TRole : MongoRole
	    {
            var dbOptions = new MongoIdentityOptions();
	        setupDatabaseAction(dbOptions);

	        var builder = services.AddIdentity<TUser, TRole>(setupIdentityAction ?? (x => { }));
	        
	        builder.AddRoleStore<RoleStore<TRole>>()
	        .AddUserStore<UserStore<TUser, TRole>>()
	        .AddUserManager<UserManager<TUser>>()
            .AddRoleManager<RoleManager<TRole>>()
	        .AddDefaultTokenProviders();


	        var userCollection =  MongoUtil.FromConnectionString<TUser>(dbOptions.ConnectionString, dbOptions.UsersCollection);
	        var roleCollection = MongoUtil.FromConnectionString<TRole>(dbOptions.ConnectionString, dbOptions.RolesCollection);

	        services.AddSingleton<IMongoCollection<TUser>>(x => userCollection);
	        services.AddSingleton<IMongoCollection<TRole>>(x => roleCollection);

	        // Identity Services
            services.AddTransient<IRoleStore<TRole>>(x => new RoleStore<TRole>(roleCollection));
            services.AddTransient<IUserStore<TUser>>(x => new UserStore<TUser, TRole>(userCollection, new RoleStore<TRole>(roleCollection), x.GetService<ILookupNormalizer>()));
	        
	        return builder;
	    }
	}
}