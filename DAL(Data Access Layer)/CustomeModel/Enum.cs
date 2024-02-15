﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class Enum
    {
            public enum Status
            {
                Unassigned = 1, Accepted = 2, Cancelled = 3, Reserving = 4, MDEnRoute = 5, MDOnSite = 6,
                FollowUp = 7, Closed = 8, Locked = 9, Declined = 10, Consult = 11, Clear = 12, CancelledByProvider = 13,
                CUploadedByClient = 14, CCApprovedByAdmin = 15
            
        }
    }
}

