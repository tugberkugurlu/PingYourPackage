namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using PingYourPackage.Domain.Entities;
    using System.Collections.Generic;

    public sealed class Configuration : DbMigrationsConfiguration<PingYourPackage.Domain.Entities.EntitiesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PingYourPackage.Domain.Entities.EntitiesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // plain: tugberk
            // salt: 7aX+miiI89BYmJ7HQlphhw==
            // pass: E5vc4dPsYpVo+IcubmKRMmzDj/ZBt/JBc1l39BUVCGc=

            // plain: EmployeeA
            // salt: nRJiG0JOfQKCZsIRtZgBwQ==
            // pass: opl6+bseDEaY6JvArJ1TJ99n6aJA/EJ9F5pMKALtdG8=

            // plain: CompanyA
            // salt: pPDYi4MNi/Ublpq5SlxmkA==
            // pass: Y8/YPnSnSwr8Zh7RVwRF4pr+AtRzr+JuHPLmkmS5Z3s=

            // plain: CompanyB
            // salt: GsjIcWEnzsnWnJ6zqF2k2Q==
            // pass: Nj8ByKH8hn356R/lDed0i3Z9hPFo4EOrCainXL6iYek=

            // plain: CompanyC
            // salt: uWp3KeQhKyHAUuK2rT1FjQ==
            // pass: K6cEbRcT1YWUvJsxD/R6oFtHJyWocZZYcdAkkWHy5pk=

            Guid[] roleKeys = new[] { 
                Guid.Parse("6d4cbf5b-8d2d-4b5a-85a2-8fb9dc841aee"), 
                Guid.Parse("ea1b58fe-1c28-4406-ac79-5e685c98eb38"), 
                Guid.Parse("361189f3-724b-4f09-9051-d0531e67b94d")
            };
            Guid[] userKeys = new[] { 
                Guid.Parse("eb3d460c-a406-41d5-bc3f-b0d93a7a7fe3"), 
                Guid.Parse("d8a739bf-df09-4506-a960-5a5809102861")
            };
            Guid[] affiliateKeys = new[] { 
                Guid.Parse("041f765a-1019-4956-b610-370d05be95ac"),
                Guid.Parse("b79e06ab-bca8-4b02-8eca-2d3e08965594"), 
                Guid.Parse("8fe72874-675a-48f7-afec-aff4e7ddc726")
            };
            Guid[] userInRoleKeys = new[] { 
                Guid.Parse("d09a4202-e789-4eb5-b08a-f9e1e7904f2d"),
                Guid.Parse("928f4dbe-ca2f-4139-9352-2894cf740bab"), 
                Guid.Parse("ea531120-1c95-4fa1-a3fc-d0c221338350"), 
                Guid.Parse("5652ea8e-bca4-4c92-8754-8786357c5914"),
                Guid.Parse("3e03df14-5079-40d9-bb1c-d4481d201b70"), 
                Guid.Parse("d707873d-357a-4c8e-8d66-51576d969863")
            };
            Guid[] shipmentTypeKeys = new[] { 
                Guid.Parse("d3dc345b-d0ce-4cdd-bc0f-cf385197b85d"),
                Guid.Parse("7c5a4abe-27c8-4df7-bfe9-be8dd0dc4df9"),
                Guid.Parse("864bc812-e25c-46b5-ae12-eddf707e77e7")
            };
            Guid[] shipmentKeys = new[] { 
                Guid.Parse("99b5554f-e405-4353-be54-a8d19b945768"),
                Guid.Parse("7c10c040-a9e8-48ec-916c-33dcc4619bed"),
                Guid.Parse("c8e416d5-31a1-4be1-87d3-d402defe6c15"),
                Guid.Parse("d1ffe887-0411-47eb-8c11-d106e26871fd"),
                Guid.Parse("6527ca96-9491-4755-a89d-3fa2a01a2d1c"),
                Guid.Parse("56252c0f-4e9f-4b02-8e71-0da6d9e539b1"),
                Guid.Parse("b4b57483-5517-4b68-bb8d-995d039c2adf"),
                Guid.Parse("d81de7d9-fe9f-4d7f-a341-5f6d3feecca7"),
                Guid.Parse("c0f246f8-7746-41f1-9414-e5900da30868"),
                Guid.Parse("fd281acf-f219-42b6-895a-5f7583708811")
            };
            Guid[] shipmentStateKeys = new[] { 
                Guid.Parse("d7657e7f-9413-406d-bff8-ecb9d43df239"),
                Guid.Parse("0b91ae65-fc8a-477c-845a-a9949d83c731"),
                Guid.Parse("9dc19811-f55b-47b6-8f95-0fde67ef90d6"),
                Guid.Parse("dcd4d887-d23f-4c80-99a0-4f8971a3e061"),
                Guid.Parse("04a1e47d-f332-4e2d-8e34-5ce231afe3a5"),
                Guid.Parse("174e4065-e39e-4b30-bc17-4f232bd2b03f"),
                Guid.Parse("c9b2f5b2-d02a-4f10-a4fe-81017ae25347"),
                Guid.Parse("fe9382c6-c18b-4676-aec0-7397cc228e81"),
                Guid.Parse("4646723f-c817-40bd-be53-e382bb149e6d"),
                Guid.Parse("7c6e978d-fd83-451a-ba03-be3dcdb7e5fa"),
                Guid.Parse("20c73d87-f6b4-484e-ae6b-ce4e43081b5d"),
                Guid.Parse("b1992b69-a1e3-4a71-8676-cb1b540ffc9e"),
                Guid.Parse("81c1606a-15bf-48b8-a705-ec4776241ba6"),
                Guid.Parse("a6b27c44-ba7c-4f86-a787-a0594df00e30"),
                Guid.Parse("33f2101f-3562-4174-ac4a-c1290d174b13"),
                Guid.Parse("2048c78e-fe59-4544-80ea-b878a6c2f5b9"),
                Guid.Parse("045bb9e5-e2b5-4696-a6ea-15113aa16307"),
                Guid.Parse("d692a7dc-0c10-4844-88ef-9c012e6bfa57"),
                Guid.Parse("b9bc27f3-68e4-4e85-bd3a-3a53d8faad3e"),
                Guid.Parse("c7ce6de3-c9cf-41c7-ad8d-70f3240f1781")
            };

            var passList = new[] { 
                new { Name = "Tugberk", PlainPass = "TugberkPass", Salt = "38YBiQq4TLbJCRdBekas5A==", Pass = "vqPogBnmZKYrBuNuh9/EBat4D3ASiGk7lzKpMykvKVU=", Base64Encoded = "VHVnYmVyazpUdWdiZXJrUGFzcw==" },
                new { Name = "EmployeeA", PlainPass = "EmployeeAPass", Salt = "PG1QB2J/qlxFWRrWZi+Oew==", Pass = "opl6+bseDEaY6JvArJ1TJ99n6aJA/EJ9F5pMKALtdG8=", Base64Encoded = "RW1wbG95ZWVBOkVtcGxveWVlQVBhc3M=" },
                new { Name = "CompanyA", PlainPass = "CompanyAPass", Salt = "olNfhyN683B4BOf68Rwkfg==", Pass = "gFx171zOge4EX/6U8YmEc7l+N1ySrgHqE8czpMj65+o=", Base64Encoded = "Q29tcGFueUE6Q29tcGFueUFQYXNz" },
                new { Name = "CompanyB", PlainPass = "CompanyBPass", Salt = "pVVEE3F9jDdwDeNW9e6B1A==", Pass = "d6nmBOFqmihaMFYQUd4Y+flJbbWtBHa9NlIbDZvDtO0=", Base64Encoded = "Q29tcGFueUI6Q29tcGFueUJQYXNz" },
                new { Name = "CompanyC", PlainPass = "CompanyCPass", Salt = "OqvZxSGklq1CGYZIy7AcMw==", Pass = "smOVBjvZHN3SUgNmSXwmbdNAjJwUP1EBKdJDpA/Pw1U=", Base64Encoded = "Q29tcGFueUM6Q29tcGFueUNQYXNz" }
            };

            context.Roles.AddOrUpdate(role => role.Key,
                new Role { Key = roleKeys[0], Name = "Admin" },
                new Role { Key = roleKeys[1], Name = "Employee" },
                new Role { Key = roleKeys[2], Name = "Affiliate" }
            );

            if (!context.Users.Any()) { 

                context.Users.AddOrUpdate(user => user.Key,
                    new User { Key = userKeys[0], Name = passList[0].Name, Email = "tugberk@example.com", Salt = passList[0].Salt, HashedPassword = passList[0].Pass, IsLocked = false, CreatedOn = DateTime.Now, Affiliate = null },
                    new User { Key = userKeys[1], Name = passList[1].Name, Email = "employeea@example.com", Salt = passList[1].Salt, HashedPassword = passList[1].Pass, IsLocked = false, CreatedOn = DateTime.Now, Affiliate = null },
                    new User { 
                        Key = affiliateKeys[0], Name = passList[2].Name, Email = "companya@example.com", Salt = passList[2].Salt, HashedPassword = passList[2].Pass, IsLocked = false, CreatedOn = DateTime.Now,
                        Affiliate = new Affiliate { Key = affiliateKeys[0], CompanyName = "Company A", TelephoneNumber = "01-123-123-1234", Address = "Company A Address", CreatedOn = DateTime.Now }
                    },
                    new User { 
                        Key = affiliateKeys[1], Name = passList[3].Name, Email = "companyb@example.com", Salt = passList[3].Salt, HashedPassword = passList[3].Pass, IsLocked = false, CreatedOn = DateTime.Now,
                        Affiliate = new Affiliate { Key = affiliateKeys[1], CompanyName = "Company B", TelephoneNumber = "01-123-123-1234", Address = "Company B Address", CreatedOn = DateTime.Now }
                    },
                    new User { 
                        Key = affiliateKeys[2], Name = passList[4].Name, Email = "companyc@example.com", Salt = passList[4].Salt, HashedPassword = passList[4].Pass, IsLocked = false, CreatedOn = DateTime.Now,
                        Affiliate = new Affiliate { Key = affiliateKeys[2], CompanyName = "Company C", TelephoneNumber = "01-123-123-1234", Address = "Company C Address", CreatedOn = DateTime.Now }
                    }
                );
            }

            context.UserInRoles.AddOrUpdate(userInRole => userInRole.Key,
                new UserInRole { Key = userInRoleKeys[0], UserKey = userKeys[0], RoleKey = roleKeys[0] },
                new UserInRole { Key = userInRoleKeys[1], UserKey = userKeys[0], RoleKey = roleKeys[1] },
                new UserInRole { Key = userInRoleKeys[2], UserKey = userKeys[1], RoleKey = roleKeys[1] },
                new UserInRole { Key = userInRoleKeys[3], UserKey = affiliateKeys[0], RoleKey = roleKeys[2] },
                new UserInRole { Key = userInRoleKeys[4], UserKey = affiliateKeys[1], RoleKey = roleKeys[2] },
                new UserInRole { Key = userInRoleKeys[5], UserKey = affiliateKeys[2], RoleKey = roleKeys[2] }
            );

            context.ShipmentTypes.AddOrUpdate(packageType => packageType.Key, 
                new ShipmentType { Key = shipmentTypeKeys[0], Name = "Small", Price = 10.00M, CreatedOn = DateTime.Now },
                new ShipmentType { Key = shipmentTypeKeys[1], Name = "Medium", Price = 20.00M, CreatedOn = DateTime.Now },
                new ShipmentType { Key = shipmentTypeKeys[2], Name = "Large", Price = 30.00M, CreatedOn = DateTime.Now }
            );

            context.Shipments.AddOrUpdate(shipment => shipment.Key,
                new Shipment { Key = shipmentKeys[0], AffiliateKey = affiliateKeys[0], ShipmentTypeKey = shipmentTypeKeys[0], Price = 23.34M, ReceiverName = "ReceiverName 1", ReceiverSurname = "ReceiverSurname 1", ReceiverEmail = "receiver1@example.com", ReceiverAddress = "Receiver 1 Addresss", ReceiverCity = "Izmir", ReceiverZipCode = "X6783Y", ReceiverCountry = "Turkey", ReceiverTelephone = "1231231212", CreatedOn = DateTime.Now },
                new Shipment { Key = shipmentKeys[1], AffiliateKey = affiliateKeys[0], ShipmentTypeKey = shipmentTypeKeys[1], Price = 23.34M, ReceiverName = "ReceiverName 2", ReceiverSurname = "ReceiverSurname 2", ReceiverEmail = "receiver2@example.com", ReceiverAddress = "Receiver 2 Addresss", ReceiverCity = "Izmir", ReceiverZipCode = "X6783Y", ReceiverCountry = "Turkey", ReceiverTelephone = "1231231212", CreatedOn = DateTime.Now },
                new Shipment { Key = shipmentKeys[2], AffiliateKey = affiliateKeys[0], ShipmentTypeKey = shipmentTypeKeys[1], Price = 36.54M, ReceiverName = "ReceiverName 3", ReceiverSurname = "ReceiverSurname 3", ReceiverEmail = "receiver3@example.com", ReceiverAddress = "Receiver 3 Addresss", ReceiverCity = "Izmir", ReceiverZipCode = "X6783Y", ReceiverCountry = "Turkey", ReceiverTelephone = "1231231212", CreatedOn = DateTime.Now },
                new Shipment { Key = shipmentKeys[3], AffiliateKey = affiliateKeys[1], ShipmentTypeKey = shipmentTypeKeys[2], Price = 23.34M, ReceiverName = "ReceiverName 4", ReceiverSurname = "ReceiverSurname 4", ReceiverEmail = "receiver4@example.com", ReceiverAddress = "Receiver 4 Addresss", ReceiverCity = "Izmir", ReceiverZipCode = "X6783Y", ReceiverCountry = "Turkey", ReceiverTelephone = "1231231212", CreatedOn = DateTime.Now },
                new Shipment { Key = shipmentKeys[4], AffiliateKey = affiliateKeys[1], ShipmentTypeKey = shipmentTypeKeys[0], Price = 23.34M, ReceiverName = "ReceiverName 5", ReceiverSurname = "ReceiverSurname 5", ReceiverEmail = "receiver5@example.com", ReceiverAddress = "Receiver 5 Addresss", ReceiverCity = "Izmir", ReceiverZipCode = "X6783Y", ReceiverCountry = "Turkey", ReceiverTelephone = "1231231212", CreatedOn = DateTime.Now },
                new Shipment { Key = shipmentKeys[5], AffiliateKey = affiliateKeys[1], ShipmentTypeKey = shipmentTypeKeys[1], Price = 73.92M, ReceiverName = "ReceiverName 6", ReceiverSurname = "ReceiverSurname 6", ReceiverEmail = "receiver6@example.com", ReceiverAddress = "Receiver 6 Addresss", ReceiverCity = "Izmir", ReceiverZipCode = "X6783Y", ReceiverCountry = "Turkey", ReceiverTelephone = "1231231212", CreatedOn = DateTime.Now },
                new Shipment { Key = shipmentKeys[6], AffiliateKey = affiliateKeys[2], ShipmentTypeKey = shipmentTypeKeys[1], Price = 13.33M, ReceiverName = "ReceiverName 7", ReceiverSurname = "ReceiverSurname 7", ReceiverEmail = "receiver7@example.com", ReceiverAddress = "Receiver 6 Addresss", ReceiverCity = "Izmir", ReceiverZipCode = "X6783Y", ReceiverCountry = "Turkey", ReceiverTelephone = "1231231212", CreatedOn = DateTime.Now },
                new Shipment { Key = shipmentKeys[7], AffiliateKey = affiliateKeys[2], ShipmentTypeKey = shipmentTypeKeys[2], Price = 23.34M, ReceiverName = "ReceiverName 8", ReceiverSurname = "ReceiverSurname 8", ReceiverEmail = "receiver8@example.com", ReceiverAddress = "Receiver 7 Addresss", ReceiverCity = "Izmir", ReceiverZipCode = "X6783Y", ReceiverCountry = "Turkey", ReceiverTelephone = "1231231212", CreatedOn = DateTime.Now },
                new Shipment { Key = shipmentKeys[8], AffiliateKey = affiliateKeys[2], ShipmentTypeKey = shipmentTypeKeys[0], Price = 23.34M, ReceiverName = "ReceiverName 9", ReceiverSurname = "ReceiverSurname 9", ReceiverEmail = "receiver9@example.com", ReceiverAddress = "Receiver 8 Addresss", ReceiverCity = "Izmir", ReceiverZipCode = "X6783Y", ReceiverCountry = "Turkey", ReceiverTelephone = "1231231212", CreatedOn = DateTime.Now },
                new Shipment { Key = shipmentKeys[9], AffiliateKey = affiliateKeys[2], ShipmentTypeKey = shipmentTypeKeys[1], Price = 52.34M, ReceiverName = "ReceiverName 10", ReceiverSurname = "ReceiverSurname 10", ReceiverEmail = "receiver10@example.com", ReceiverAddress = "Receiver 9 Addresss", ReceiverCity = "Izmir", ReceiverZipCode = "X6783Y", ReceiverCountry = "Turkey", ReceiverTelephone = "1231231212", CreatedOn = DateTime.Now }
            );

            context.ShipmentStates.AddOrUpdate(shipmentState => shipmentState.Key,
                new ShipmentState { Key = shipmentStateKeys[0], ShipmentKey = shipmentKeys[0], ShipmentStatus = ShipmentStatus.Ordered, CreatedOn = DateTime.Now.AddDays(-20) },
                new ShipmentState { Key = shipmentStateKeys[1], ShipmentKey = shipmentKeys[0], ShipmentStatus = ShipmentStatus.Scheduled, CreatedOn = DateTime.Now.AddDays(-19) },
                new ShipmentState { Key = shipmentStateKeys[2], ShipmentKey = shipmentKeys[0], ShipmentStatus = ShipmentStatus.InTransit, CreatedOn = DateTime.Now.AddDays(-18) },
                new ShipmentState { Key = shipmentStateKeys[3], ShipmentKey = shipmentKeys[1], ShipmentStatus = ShipmentStatus.Ordered, CreatedOn = DateTime.Now.AddDays(-17).AddHours(4) },
                new ShipmentState { Key = shipmentStateKeys[4], ShipmentKey = shipmentKeys[2], ShipmentStatus = ShipmentStatus.Ordered, CreatedOn = DateTime.Now.AddDays(-17) },
                new ShipmentState { Key = shipmentStateKeys[5], ShipmentKey = shipmentKeys[2], ShipmentStatus = ShipmentStatus.Scheduled, CreatedOn = DateTime.Now.AddDays(-16) },
                new ShipmentState { Key = shipmentStateKeys[6], ShipmentKey = shipmentKeys[3], ShipmentStatus = ShipmentStatus.Ordered, CreatedOn = DateTime.Now.AddDays(-15).AddHours(2) },
                new ShipmentState { Key = shipmentStateKeys[7], ShipmentKey = shipmentKeys[3], ShipmentStatus = ShipmentStatus.Scheduled, CreatedOn = DateTime.Now.AddDays(-15) },
                new ShipmentState { Key = shipmentStateKeys[8], ShipmentKey = shipmentKeys[3], ShipmentStatus = ShipmentStatus.InTransit, CreatedOn = DateTime.Now.AddDays(-14) },
                new ShipmentState { Key = shipmentStateKeys[9], ShipmentKey = shipmentKeys[3], ShipmentStatus = ShipmentStatus.Delivered, CreatedOn = DateTime.Now.AddDays(-14).AddHours(3) },
                new ShipmentState { Key = shipmentStateKeys[10], ShipmentKey = shipmentKeys[4], ShipmentStatus = ShipmentStatus.Ordered, CreatedOn = DateTime.Now.AddDays(-5).AddHours(2) },
                new ShipmentState { Key = shipmentStateKeys[11], ShipmentKey = shipmentKeys[4], ShipmentStatus = ShipmentStatus.Scheduled, CreatedOn = DateTime.Now.AddDays(-15).AddHours(16) },
                new ShipmentState { Key = shipmentStateKeys[12], ShipmentKey = shipmentKeys[5], ShipmentStatus = ShipmentStatus.Ordered, CreatedOn = DateTime.Now.AddDays(-12).AddHours(11) },
                new ShipmentState { Key = shipmentStateKeys[13], ShipmentKey = shipmentKeys[6], ShipmentStatus = ShipmentStatus.Ordered, CreatedOn = DateTime.Now.AddDays(-7).AddHours(11) },
                new ShipmentState { Key = shipmentStateKeys[14], ShipmentKey = shipmentKeys[6], ShipmentStatus = ShipmentStatus.InTransit, CreatedOn = DateTime.Now.AddDays(-8).AddHours(2) },
                new ShipmentState { Key = shipmentStateKeys[15], ShipmentKey = shipmentKeys[7], ShipmentStatus = ShipmentStatus.Ordered, CreatedOn = DateTime.Now.AddDays(-1).AddHours(3) },
                new ShipmentState { Key = shipmentStateKeys[16], ShipmentKey = shipmentKeys[8], ShipmentStatus = ShipmentStatus.Ordered, CreatedOn = DateTime.Now.AddDays(-1).AddHours(19) },
                new ShipmentState { Key = shipmentStateKeys[17], ShipmentKey = shipmentKeys[9], ShipmentStatus = ShipmentStatus.Ordered, CreatedOn = DateTime.Now.AddDays(-1).AddHours(22) }
            );
        }
    }
}