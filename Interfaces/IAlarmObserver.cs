﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Interfaces
{
    public interface IAlarmObserver
    {
        public void Update();
    }
}