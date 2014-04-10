using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK
{
	public class Project
	{
		public JiraEnvironment JiraEnvironment { get; set; }

		public String Key { get; set; }
		public String Name { get; set; }

		private User _lead;

		public User Lead { get; set; }

		public User ProjectLead
		{
			get { return _lead ?? (_lead = JiraEnvironment.Client.GetUser(Lead.Username)); }
		}

		private List<User> _assignableUsers;
		public List<User> AssignableUsers
		{
			get
			{
				return _assignableUsers ??
					   (_assignableUsers =
							JiraEnvironment.Client.GetAssignableUsers(this.Key));
			}
		}

		private List<ProjectVersion> _projectVersions;
		public List<ProjectVersion> ProjectVersions
		{
			get
			{
				if (_projectVersions == null)
				{
					_projectVersions =
						   JiraEnvironment.Client.GetProjectVersions(this.Key);

					_projectVersions.ForEach(vers => vers.Project = this);
				}
				return _projectVersions;
			}
		}

		public ProjectVersion PreviousVersion
		{
			get
			{
				return
					ProjectVersions.Where(vers => vers.ReleaseDate.CompareTo(DateTime.Now) <= 0)
						.OrderByDescending(vers => vers.ReleaseDate)
						.FirstOrDefault();
			}
		}

		public ProjectVersion CurrentVersion
		{
			get
			{
				return ProjectVersions.FirstOrDefault(
					vers => vers.StartDate.CompareTo(DateTime.Now) <= 0 && vers.ReleaseDate.CompareTo(DateTime.Now) > 0 && !vers.Archived);
			}
		}

		public ProjectVersion NextVersion
		{
			get
			{
				return ProjectVersions.Where(vers => vers.StartDate.CompareTo(DateTime.Now) > 0 && vers.ReleaseDate.CompareTo(DateTime.Now) > 0).OrderBy(vers => vers.StartDate).FirstOrDefault();
			}
		}
	}
}
