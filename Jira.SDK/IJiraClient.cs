using Jira.SDK.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK
{
	public interface IJiraClient
	{
		List<Project> GetProjects();
		Project GetProject(String projectKey);

		List<Field> GetFields();

		List<ProjectVersion> GetProjectVersions(String projectKey);

        List<ProjectComponent> GetProjectComponents(String projectKey);

		User GetUser(String username);
		List<User> GetAssignableUsers(String projectKey);

		List<AgileBoard> GetAgileBoards();
		List<Sprint> GetSprintsFromAgileBoard(Int32 agileBoardID);
		List<Sprint> GetBacklogSprintsFromAgileBoard(Int32 agileBoardID);
		Sprint GetSprint(Int32 agileBoardID, Int32 sprintID);
		List<Issue> GetIssuesFromSprint(Int32 sprintID);

		Issue GetIssue(String key);
		List<Issue> SearchIssues(String jql);

		Issue AddIssue(IssueFields fields);
		Comment AddCommentToIssue(Issue issue, Comment comment);
        void TransitionIssue(Issue issue, Transition transition, Comment comment);

		List<Issue> GetIssuesFromProjectVersion(String projectKey, String projectVersionName);
		List<Issue> GetSubtasksFromIssue(String issueKey);
		WorklogSearchResult GetWorkLogs(String issueKey);
        List<Transition> GetTransitions(String issueKey);

		List<Issue> GetEpicIssuesFromProject(String projectName);
		Issue GetEpicIssueFromProject(String projectName, String epicName);

		List<Issue> GetIssuesWithEpicLink(String epicLink);

        List<IssueFilter> GetFavoriteFilters();
	}
}
