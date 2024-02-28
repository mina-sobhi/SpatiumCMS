﻿using Domain.ApplicationUserAggregate;
using Domain.Base;

namespace Domain.BlogsAggregate
{
    public class Like : EntityBase
    {
        public bool IsLike { get; private set; }
        public string CreatedbyId { get; private set; }
        public int PostId { get; private set; }

        public virtual ApplicationUser Createdby { get; private set; }
        public virtual Post Post { get; private set; }
        #region Ctor
        public Like()
        {
            
        }
        #endregion
    }
}