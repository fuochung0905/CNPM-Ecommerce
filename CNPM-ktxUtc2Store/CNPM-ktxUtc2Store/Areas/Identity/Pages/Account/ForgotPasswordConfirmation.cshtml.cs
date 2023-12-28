﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CNPM_ktxUtc2Store.Areas.Identity.Pages.Account
{

    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {
        
        public void OnGet()
        {
        }
    }
}
