﻿using Billiard.DataLayer.Context;
using Billiard.Entities.Identity;
using Billiard.Services.Contracts.Identity;
using Billiard.ViewModels.Identity;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using DNTCommon.Web.Core;

namespace Billiard.Services.Identity
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2578
    /// </summary>
    public class ApplicationUserManager :
        UserManager<User>,
        IApplicationUserManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _uow;
        private readonly IUsedPasswordsService _usedPasswordsService;
        private readonly IdentityErrorDescriber _errors;
        private readonly ILookupNormalizer _keyNormalizer;
        private readonly ILogger<ApplicationUserManager> _logger;
        private readonly IOptions<IdentityOptions> _optionsAccessor;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IServiceProvider _services;
        private readonly DbSet<User> _users;
        private readonly DbSet<Role> _roles;
        private readonly IApplicationUserStore _userStore;
        private readonly IEnumerable<IUserValidator<User>> _userValidators;
        private User _currentUserInScope;

        public ApplicationUserManager(
            IApplicationUserStore store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<ApplicationUserManager> logger,
            IHttpContextAccessor contextAccessor,
            IUnitOfWork uow,
            IUsedPasswordsService usedPasswordsService)
            : base(
                (UserStore<User, Role, ApplicationDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>)store,
                  optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _userStore = store ?? throw new ArgumentNullException(nameof(_userStore));
            _optionsAccessor = optionsAccessor ?? throw new ArgumentNullException(nameof(_optionsAccessor));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(_passwordHasher));
            _userValidators = userValidators ?? throw new ArgumentNullException(nameof(_userValidators));
            _passwordValidators = passwordValidators ?? throw new ArgumentNullException(nameof(_passwordValidators));
            _keyNormalizer = keyNormalizer ?? throw new ArgumentNullException(nameof(_keyNormalizer));
            _errors = errors ?? throw new ArgumentNullException(nameof(_errors));
            _services = services ?? throw new ArgumentNullException(nameof(_services));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _usedPasswordsService = usedPasswordsService ?? throw new ArgumentNullException(nameof(_usedPasswordsService));
            _users = uow.Set<User>();
            _roles = uow.Set<Role>();
        }

        #region BaseClass

        string IApplicationUserManager.CreateTwoFactorRecoveryCode()
        {
            return base.CreateTwoFactorRecoveryCode();
        }

        Task<PasswordVerificationResult> IApplicationUserManager.VerifyPasswordAsync(IUserPasswordStore<User> store, User user, string password)
        {
            return base.VerifyPasswordAsync(store, user, password);
        }

        public override async Task<IdentityResult> CreateAsync(User user)
        {
            var result = await base.CreateAsync(user);
            if (result.Succeeded)
            {
                await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
            }
            return result;
        }

        public override async Task<IdentityResult> CreateAsync(User user, string password)
        {
            var result = await base.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
            }
            return result;
        }

        public override async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            var result = await base.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {
                await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
            }
            return result;
        }

        public override async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
        {
            var result = await base.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
            }
            return result;
        }

        #endregion

        #region CustomMethods

        public User FindById(int userId)
        {
            return _users.Find(userId);
        }

        public Task<User> FindByIdIncludeUserRolesAsync(int userId)
        {
            return _users.Include(x => x.Roles).Include(x => x.UserBankCards).Include(x => x.UserBankShebas).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return Users.ToListAsync();
        }

        public User GetCurrentUser()
        {
            if (_currentUserInScope != null)
            {
                return _currentUserInScope;
            }

            var currentUserId = GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return null;
            }

            var userId = int.Parse(currentUserId);
            return _currentUserInScope = FindById(userId);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            return _currentUserInScope ??
                (_currentUserInScope = await GetUserAsync(_contextAccessor.HttpContext.User));
        }

        public string GetCurrentUserId()
        {
            return _contextAccessor.HttpContext.User.Identity.GetUserId();
        }

        public int? CurrentUserId
        {
            get
            {
                var userId = _contextAccessor.HttpContext.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                return !int.TryParse(userId, out int result) ? (int?)null : result;
            }
        }

        IPasswordHasher<User> IApplicationUserManager.PasswordHasher { get => base.PasswordHasher; set => base.PasswordHasher = value; }

        IList<IUserValidator<User>> IApplicationUserManager.UserValidators => base.UserValidators;

        IList<IPasswordValidator<User>> IApplicationUserManager.PasswordValidators => base.PasswordValidators;

        IQueryable<User> IApplicationUserManager.Users => base.Users;

        public string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext.User.Identity.GetUserName();
        }

        public async Task<bool> HasPasswordAsync(int userId)
        {
            var user = await FindByIdAsync(userId.ToString());
            return user?.PasswordHash != null;
        }

        public async Task<bool> HasPhoneNumberAsync(int userId)
        {
            var user = await FindByIdAsync(userId.ToString());
            return user?.PhoneNumber != null;
        }

        public async Task<byte[]> GetEmailImageAsync(int? userId)
        {
            if (userId == null)
                return "?".TextToImage(new TextToImageOptions());

            var user = await FindByIdAsync(userId.Value.ToString());
            if (user == null)
                return "?".TextToImage(new TextToImageOptions());

            if (!user.IsEmailPublic)
                return "?".TextToImage(new TextToImageOptions());

            return user.Email.TextToImage(new TextToImageOptions());
        }

        public async Task<PagedUsersListViewModel> GetPagedUsersListAsync(SearchUsersViewModel model, int pageNumber)
        {
            var skipRecords = pageNumber * model.MaxNumberOfRows;
            var query = _users.Include(x => x.Roles).AsNoTracking();

            if (!model.ShowAllUsers)
            {
                query = query.Where(x => x.IsActive == model.UserIsActive);
            }

            if (!string.IsNullOrWhiteSpace(model.TextToFind))
            {
                model.TextToFind = model.TextToFind.ApplyCorrectYeKe();

                if (model.IsPartOfEmail)
                {
                    query = query.Where(x => x.Email.Contains(model.TextToFind));
                }

                if (model.IsUserId)
                {
                    if (int.TryParse(model.TextToFind, out int userId))
                    {
                        query = query.Where(x => x.Id == userId);
                    }
                }

                if (model.IsPartOfName)
                {
                    query = query.Where(x => x.FirstName.Contains(model.TextToFind));
                }

                if (model.IsPartOfLastName)
                {
                    query = query.Where(x => x.LastName.Contains(model.TextToFind));
                }

                if (model.IsPartOfUserName)
                {
                    query = query.Where(x => x.UserName.Contains(model.TextToFind));
                }

                if (model.IsPartOfLocation)
                {
                    query = query.Where(x => x.Location.Contains(model.TextToFind));
                }
            }

            if (model.HasEmailConfirmed)
            {
                query = query.Where(x => x.EmailConfirmed);
            }

            if (model.UserIsLockedOut)
            {
                query = query.Where(x => x.LockoutEnd != null);
            }

            if (model.HasTwoFactorEnabled)
            {
                query = query.Where(x => x.TwoFactorEnabled);
            }

            query = query.OrderBy(x => x.Id);
            return new PagedUsersListViewModel
            {
                Paging =
                {
                    TotalItems = await query.CountAsync()
                },
                Users = await query.Skip(skipRecords).Take(model.MaxNumberOfRows).ToListAsync(),
                Roles = await _roles.ToListAsync()
            };
        }

        public async Task<PagedUsersListViewModel> GetPagedUsersListAsync(
            int pageNumber, int recordsPerPage,
            string sortByField, SortOrder sortOrder,
            bool showAllUsers)
        {
            var skipRecords = pageNumber * recordsPerPage;
            var query = _users.Include(x => x.Roles).AsNoTracking();

            if (!showAllUsers)
            {
                query = query.Where(x => x.IsActive);
            }

            switch (sortByField)
            {
                case nameof(User.Id):
                    query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
                    break;
                default:
                    query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
                    break;
            }

            return new PagedUsersListViewModel
            {
                Paging =
                {
                    TotalItems = await query.CountAsync()
                },
                Users = await query.Skip(skipRecords).Take(recordsPerPage).ToListAsync(),
                Roles = await _roles.ToListAsync()
            };
        }

        public async Task<IdentityResult> UpdateUserAndSecurityStampAsync(int userId, Action<User> action)
        {
            var user = await FindByIdIncludeUserRolesAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }

            action(user);

            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UpdateSecurityStampAsync(user);
        }

        public async Task<IdentityResult> AddOrUpdateUserRolesAsync(int userId, IList<int> selectedRoleIds, Action<User> action = null)
        {
            var user = await FindByIdIncludeUserRolesAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }

            var currentUserRoleIds = user.Roles.Select(x => x.RoleId).ToList();

            if (selectedRoleIds == null)
            {
                selectedRoleIds = new List<int>();
            }

            var newRolesToAdd = selectedRoleIds.Except(currentUserRoleIds).ToList();
            foreach (var roleId in newRolesToAdd)
            {
                user.Roles.Add(new UserRole { RoleId = roleId, UserId = user.Id });
            }

            var removedRoles = currentUserRoleIds.Except(selectedRoleIds).ToList();
            foreach (var roleId in removedRoles)
            {
                var userRole = user.Roles.SingleOrDefault(ur => ur.RoleId == roleId);
                if (userRole != null)
                {
                    user.Roles.Remove(userRole);
                }
            }

            action?.Invoke(user);

            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UpdateSecurityStampAsync(user);
        }

        Task<IdentityResult> IApplicationUserManager.UpdatePasswordHash(User user, string newPassword, bool validatePassword)
        {
            return base.UpdatePasswordHash(user, newPassword, validatePassword);
        }
        public async Task<IdentityResult> AddMobile(int userId, string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "EmptyMobile",
                    Description = "شماره موبایل را وارد نمایید"
                });
            }
            var user = await FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }
            if (user.MobileNumber == mobile && user.IsMobileNumberConfirmed == isConfirmed.confirmed)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "MobileRepetitious",
                    Description = "شماره موبایل قبلا تایید شده است."
                });
            }
            var alluser = await GetAllUsersAsync();
            var isexist = alluser.Where(c => c.MobileNumber == mobile && c.IsMobileNumberConfirmed == isConfirmed.confirmed).Select(c => c).Any();
            if (isexist)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "MobileUsed",
                    Description = "شماره موبایل قبلا توسط کاربر دیگری تایید شده است."
                });
            }
            user.MobileNumber = mobile;
            user.IsMobileNumberConfirmed = isConfirmed.waitingforconfirmation;
            user.UserChangeInfo = true;
            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            else
            {
                return IdentityResult.Success;
            }
        }

        public async Task<IdentityResult> ConfirmMobile(int userId, isConfirmed confirmed)
        {
            var user = await FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }
            if (string.IsNullOrEmpty(user.MobileNumber))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "MobileNotFound",
                    Description = "لطفا ابتدا شماره موبایل خود را وارد نمایید."
                });
            }
            if (user.IsMobileNumberConfirmed == isConfirmed.confirmed)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "MobileRepetitious",
                    Description = "شماره موبایل قبلا تایید شده است."
                });
            }
            user.IsMobileNumberConfirmed = confirmed;
            user.UserChangeInfo = false;
            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            else
            {
                return IdentityResult.Success;
            }
        }

        public async Task<IdentityResult> AddDocFile(int userId, string docfilename)
        {
            var user = await FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }
            if (user.IsDocPhotoConfirmed == isConfirmed.confirmed)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "docfilename",
                    Description = "مدرک شناسایی قبلا تایید شده است."
                });
            }

            user.DocPhotoFileName = docfilename;
            user.IsDocPhotoConfirmed = isConfirmed.waitingforconfirmation;
            user.UserChangeInfo = false;
            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            else
            {
                return IdentityResult.Success;
            }
        }

        public async Task<IdentityResult> ConfirmDocFile(int userId, isConfirmed confirmed, string description)
        {
            var user = await FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }

            if (user.IsDocPhotoConfirmed == isConfirmed.confirmed)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DocRepetitious",
                    Description = "مدرک شناسایی شما قبلا تایید شده است"
                });
            }
            user.IsDocPhotoConfirmed = confirmed;
            user.DocDescription = description;
            user.UserChangeInfo = false;
            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            else
            {
                return IdentityResult.Success;
            }
        }

        public async Task<IdentityResult> AddSelfiFile(int userId, string selfifilename)
        {
            var user = await FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }
            if (user.IsSelfiPhotoConfirmed == isConfirmed.confirmed)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "selfifilename",
                    Description = "تصویر سلفی قبلا تایید شده است."
                });
            }

            user.SelfiPhotoFileName = selfifilename;
            user.IsSelfiPhotoConfirmed = isConfirmed.waitingforconfirmation;
            user.UserChangeInfo = true;
            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            else
            {
                return IdentityResult.Success;
            }
        }

        public async Task<IdentityResult> ConfirmSelfiFile(int userId, isConfirmed confirmed, string description)
        {
            var user = await FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }


            user.IsSelfiPhotoConfirmed = confirmed;
            user.SelfiDescription = description;
            user.UserChangeInfo = false;
            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            else
            {
                return IdentityResult.Success;
            }
        }

        public async Task<IdentityResult> AddAddress(int userId, string address, string postalcode, string code)
        {
            var user = await FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }

            user.PostalCode = postalcode;
            user.LocationCode = code;
            user.IsPostalCodeConfirmed = isConfirmed.waitingforconfirmation;
            user.Location = address;
            user.IsLocationConfirmed = isConfirmed.waitingforconfirmation;
            user.UserChangeInfo = true;
            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            else
            {
                return IdentityResult.Success;
            }
        }

        public async Task<IdentityResult> ConfirmAddress(int userId, isConfirmed confirmed, string description)
        {
            var user = await FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }

            user.IsPostalCodeConfirmed = confirmed;
            user.IsLocationConfirmed = confirmed;
            user.AddressDescription = description + Environment.NewLine;
            user.UserChangeInfo = false;
            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            else
            {
                return IdentityResult.Success;
            }
        }

        public async Task<IdentityResult> AddPhoneNumber(int userId, string Phone, string code)
        {
            if (string.IsNullOrWhiteSpace(Phone))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "EmptyMobile",
                    Description = "شماره تلفن را وارد نمایید"
                });
            }
            var user = await FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }
            if (user.HomePhoneNumber == Phone && user.IsPhoneNumberConfirmed == isConfirmed.confirmed)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "MobileRepetitious",
                    Description = "شماره تلفن قبلا تایید شده است."
                });
            }
            var alluser = await GetAllUsersAsync();
            user.HomePhoneNumberCode = code;
            user.HomePhoneNumber = Phone;
            user.IsPhoneNumberConfirmed = isConfirmed.waitingforconfirmation;
            user.UserChangeInfo = true;
            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            else
            {
                return IdentityResult.Success;
            }
        }

        public async Task<IdentityResult> ConfirmPhoneNumber(int userId, isConfirmed confirmed, string description)
        {
            var user = await FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "کاربر مورد نظر یافت نشد."
                });
            }


            user.IsPhoneNumberConfirmed = confirmed;
            user.HomePhoneDescription = description;
            user.UserChangeInfo = false;
            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            else
            {
                return IdentityResult.Success;
            }
        }




        #endregion
    }
}