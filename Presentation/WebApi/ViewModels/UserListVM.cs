using Application.DTOs;
using Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class UserListVM
    {
        public Guid? role { get; set; }
        public PaginatedResult<UsersListResponseDTO> ListWithPagination { get; set; }
    }
}
