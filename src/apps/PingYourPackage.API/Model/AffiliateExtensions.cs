using PingYourPackage.API.Model.Dtos;
using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model {
    
    internal static class AffiliateExtensions {

        internal static AffiliateDto ToAffiliateDto(this Affiliate affiliate) {

            return new AffiliateDto {
                Key = affiliate.Key,
                CompanyName = affiliate.CompanyName,
                Address = affiliate.Address,
                TelephoneNumber = affiliate.TelephoneNumber,
                CreatedOn = affiliate.CreatedOn,
                MemberInfo = new AffiliateMemberInfoDto { 
                    UserName = affiliate.User.Name,
                    Email = affiliate.User.Name,
                    IsLocked = affiliate.User.IsLocked,
                    CreatedOn = affiliate.User.CreatedOn,
                    LastUpdatedOn = affiliate.User.LastUpdatedOn
                }
            };
        }
    }
}