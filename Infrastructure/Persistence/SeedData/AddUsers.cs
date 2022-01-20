using Application.Authorization;
using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.SeedData
{
    public class AddUsers
    {
        public static async Task SeedData(IIdentityService identityService)
        {
            var hasUsers = identityService.Entities.Any();

            if (!hasUsers)
            {
                #region Admins
                var user = new AppUser(Name.Create("Eyas", "Khalaf"), "eyas.khalaf@siemens-healthineers.com", "eyas.khalaf@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.ADMIN.ToString());

                user = new AppUser(Name.Create("hadeel", "alshareef"), "hadeel.alshareef@siemens-healthineers.com", "hadeel.alshareef@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.ADMIN.ToString());
                #endregion

                #region HR
                user = new AppUser(Name.Create("Baber", "Hussain"), "baber.hussain@siemens-healthineers.com", "baber.hussain@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Mohamed", "Hanafy"), "mohamed.hanafy@siemens-healthineers.com", "mohamed.hanafy@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Albaraa", "Khoja"), "albaraa.khoja@siemens-healthineers.com", "albaraa.khoja@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Mohamed", "Sawtari"), "mohamed.sawtari@siemens-healthineers.com", "mohamed.sawtari@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Mustafa", "Qadadah"), "mustafa.qadadah@siemens-healthineers.com", "mustafa.qadadah@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Ikbal", "Daboul"), "ikbal.daboul@siemens-healthineers.com", "ikbal.daboul@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Fadi", "Abu Shaar"), "fadi.abushaar@siemens-healthineers.com", "fadi.abushaar@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Mahmoud", "Elsayed"), "elsayed.mahmoud@siemens-healthineers.com", "elsayed.mahmoud@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Khaldoun", "Touban"), "khaldoun.touban@siemens-healthineers.com", "khaldoun.touban@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Bader", "Alanezi"), "bader.alanezi@siemens-healthineers.com", "bader.alanezi@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Manahel", "Alqadeeb"), "manahel.alqadeeb@siemens-healthineers.com", "manahel.alqadeeb@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Anas", "Hallak"), "anas.hallak@siemens-healthineers.com", "anas.hallak@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Ali", "Zhairati"), "ali.zhairati@siemens-healthineers.com", "ali.zhairati@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Ahmed", "Ibrahim"), "ahmed.ibrahim@siemens-healthineers.com", "ahmed.ibrahim@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Michel", "Atallah"), "michel.atallah@siemens-healthineers.com", "michel.atallah@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Rainer", "Lang"), "lang.rainer@siemens-healthineers.com", "lang.rainer@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Yaseen", "Alhamad"), "yaseen.alhamad@siemens-healthineers.com", "yaseen.alhamad@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());

                user = new AppUser(Name.Create("Ehab", "Wali"), "ehab.wali@siemens-healthineers.com", "ehab.wali@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.VIEW_ONLY.ToString());
                #endregion

                #region Blocked
                user = new AppUser(Name.Create("Ayman", "Tamimi"), "ayman.tamimi@siemens-healthineers.com", "ayman.tamimi@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Eirfan", "Adnan"), "eirfan.adnan@siemens-healthineers.com", "eirfan.adnan@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Mohammed", "Al Mutair"), "mohammed.al-mutairi@siemens-healthineers.com", "mohammed.al-mutairi@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Ammar", "Alrefaaei"), "ammar.alrefaaei@siemens-healthineers.com", "ammar.alrefaaei@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Mohammed", "Al-Gharibeh"), "mohammed.al-gharibeh@siemenshealthineers.com", "mohammed.al-gharibeh@siemenshealthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Bader", "Mutwali"), "bader.mutwali@siemens-healthineers.com", "bader.mutwali@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Wesam", "Jadallah"), "wesam.jadallah@siemens-healthineers.com", "wesam.jadallah@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Loay", "Khamis"), "loay.khamis@siemens-healthineers.com", "loay.khamis@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Mouaz", "Mouaz"), "mouaz.alali@siemens-healthineers.com", "mouaz.alali@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Qusaie", "Qusaie"), "qusaie.rihan@siemens-healthineers.com", "qusaie.rihan@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Suzan", "Alotaibi"), "suzan.alotaibi@siemens-healthineers.com", "suzan.alotaibi@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Saeed", "Alshahrani"), "saeed.alshahrani@siemens-healthineers.com", "saeed.alshahrani@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Anas", "Sindi"), "anas.sindi@siemens-healthineers.com", "anas.sindi@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Ahmed", "Alanazi"), "ahmed.alanazi@siemens-healthineers.com", "ahmed.alanazi@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Mohammed", "Hakami"), "mohammed.hakami@siemens-healthineers.com", "mohammed.hakami@siemens-healthineers.com");
                await identityService.Add(user);

                user = new AppUser(Name.Create("Samir", "Slimani"), "samir.slimani@siemens-healthineers.com", "samir.slimani@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Alhamed", "Saud"), "saud.alhamed@siemens-healthineers.com", "saud.alhamed@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Najla", "Alhasan"), "najla.alhasan@siemens-healthineers.com", "najla.alhasan@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Aladin", "Eltayeb"), "aladin.eltayeb@siemens-healthineers.com", "aladin.eltayeb@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Amro", "Hassoubah"), "amro.hassoubah@siemens-healthineers.com", "amro.hassoubah@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Asad", "Awwad"), "asad.awwad@siemens-healthineers.com", "asad.awwad@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Siraj", "Ali"), "siraj.ali@siemens-healthineers.com", "siraj.ali@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Rases", "Alwihbe"), "rases.alwhibe@siemens-healthineers.com", "rases.alwhibe@siemens-healthineers.com");
                await identityService.Add(user);

                user = new AppUser(Name.Create("Umer", "Hussain"), "Umer.Hussain@siemens-healthineers.com", "Umer.Hussain@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Youssef", "Lhaila"), "youssef.lhaila@siemens-healthineers.com", "youssef.lhaila@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Sultan", "Alghamdi"), "sultan.alghamdi@siemens-healthineers.com", "sultan.alghamdi@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Ahmed", "Ahmed"), "mansoor.ahmed@siemens-healthineers.com", "mansoor.ahmed@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                user = new AppUser(Name.Create("Ahmed", "Sedqi"), "ahmed.sedqi@siemens-healthineers.com", "ahmed.sedqi@siemens-healthineers.com");
                await identityService.Add(user);
                await identityService.AddUserToRole(user, DefaultRoles.BLOCKED.ToString());

                #endregion
            }
        }
    }
}
