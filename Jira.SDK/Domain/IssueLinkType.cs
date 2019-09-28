﻿using System;

namespace Jira.SDK.Domain
{
    public class IssueLinkType
	{
		public enum IssueLinkTypeEnum
		{
			Cloners,
			Relates,
			Blocks,
			Duplicate,
		}

		public Int32 ID { get; set; }
		public String Name { get; set; }

		public IssueLinkTypeEnum ToEnum()
		{
			return (IssueLinkTypeEnum)Enum.Parse(typeof(IssueLinkTypeEnum), Name);
		}
	}
}
