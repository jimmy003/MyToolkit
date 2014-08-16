﻿//-----------------------------------------------------------------------
// <copyright file="GoBackMessage.cs" company="MyToolkit">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>http://mytoolkit.codeplex.com/license</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;

namespace MyToolkit.Messaging
{
    //// TODO: Implement CallbackMessage

	public class GoBackMessage
	{
		public Action<bool> Completed { get; set; }
	}
}
