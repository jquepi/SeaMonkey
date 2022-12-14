using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using System;
using System.Collections.Generic;
using Serilog;

namespace SeaMonkey.Monkeys
{
    public class ConfigurationMonkey : Monkey
    {
        public ConfigurationMonkey(OctopusRepository repository) : base(repository)
        {
        }

        public void CreateRecords(int numberOfSubscriptions,
            int numberOfTeams,
            int numberOfUsers,
            int numberOfUserRoles)
        {
            CreateDisabledSubscriptions(numberOfSubscriptions);
            CreateEmptyTeams(numberOfTeams);
            CreateInactiveUsers(numberOfUsers);
            CreateEmptyUserRoles(numberOfUserRoles);
        }

        #region Subscriptions

        public void CreateDisabledSubscriptions(int numberOfRecords)
        {
            Log.Information("Creating {n} disabled subscriptions", numberOfRecords);
            var currentCount = Repository.Subscriptions.FindAll().Count();
            Enumerable.Range(currentCount, numberOfRecords)
                .AsParallel()
                .ForAll(i => CreateDisabledSubscription(i));
        }

        private SubscriptionResource CreateDisabledSubscription(int prefix)
        {
            return
                Repository.Subscriptions.Create(new SubscriptionResource()
                {
                    Name = "Subscription-" + prefix.ToString("000"),
                    Type = SubscriptionType.Event,
                    IsDisabled = true, // We're just load testing some data, this can be disabled.
                    EventNotificationSubscription = new EventNotificationSubscription
                    {
                        Filter = new EventNotificationSubscriptionFilter
                        {

                        },
                        EmailTeams = new ReferenceCollection(new string[] { "teams-administrators" }),
                        EmailFrequencyPeriod = TimeSpan.Parse("99.00:00:00"),
                        EmailShowDatesInTimeZoneId = "E. Australia Standard Time",
                    }
                });
        }

        #endregion

        #region Teams

        public void CreateEmptyTeams(int numberOfRecords)
        {
            Log.Information("Creating {n} empty teams", numberOfRecords);
            var currentCount = Repository.Teams.FindAll().Count();
            Enumerable.Range(currentCount, numberOfRecords)
                .AsParallel()
                .ForAll(i => CreateEmptyTeam(i));
        }

        private TeamResource CreateEmptyTeam(int prefix)
        {
            return
                Repository.Teams.Create(new TeamResource()
                {
                    Name = "Team-" + prefix.ToString("000"),
                });
        }

        #endregion

        #region Users

        public void CreateInactiveUsers(int numberOfRecords)
        {
            Log.Information("Creating {n} inactive users", numberOfRecords);
            var users = Repository.Users.FindAll();
            var currentCount = Repository.Users.FindAll().Count();
            Enumerable.Range(currentCount, numberOfRecords)
                .AsParallel()
                .ForAll(i => CreateInactiveUser(i));
        }

        private UserResource CreateInactiveUser(int prefix)
        {
            return
                Repository.Users.Create(new UserResource()
                {
                    Username = "User-" + prefix.ToString("000"),
                    DisplayName = "User-" + prefix.ToString("000"),
                    IsActive = false,
                    IsService = false,
                    EmailAddress = "rick-" + prefix.ToString("000") + "@morty.com",
                    Password = "RickAndMortyForPresident"
                });
    }

        #endregion

        #region UserRoles

        public void CreateEmptyUserRoles(int numberOfRecords)
        {
            Log.Information("Creating {n} roles", numberOfRecords);
            var currentCount = Repository.UserRoles.FindAll().Count();
            Enumerable.Range(currentCount, numberOfRecords)
                .AsParallel()
                .ForAll(i => CreateEmptyUserRole(i));
        }

        private UserRoleResource CreateEmptyUserRole(int prefix)
        {
            return
                Repository.UserRoles.Create(new UserRoleResource()
                {
                    Name = "UserRole-" + prefix.ToString("000"),
                    Description = "They're robots Morty! It's okay to shoot them! They're just robots!",
                    CanBeDeleted = true,
                    SupportedRestrictions = new List<string>(),
                    SpacePermissionDescriptions = new List<string>(),
                    GrantedSpacePermissions = new List<Permission>(),
                    SystemPermissionDescriptions = new List<string>(),
                    GrantedSystemPermissions = new List<Permission>()
                });
        }

        #endregion
    }
}
