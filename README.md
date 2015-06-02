Jira.SDK
========

SDK for Jira integration in .net


<h2>Open issues</h2>
* Not all code is unit tested
* Some custom business logic still in the API (especialy in the Epic class).

This repository is open for pull requests!

<h2>Nuget</h2>
You can download the nuget file here: https://www.nuget.org/packages/Jira.SDK/1.1.1.2

<h3>Version history</h3>
<h4>New features</h4>
	* Added methods to collect the linked issues of a specific issue.
	* Added support for custom fields for Issues.
	* Added support for labels, you can now get all labels linked to a specific issue.
	
	<h4>Bugfixes</h4>
	* Fixed some issues with dates (for example, the resolution date).
	* Fixed a bug where the active sprint was listed twice when getting all sprints from an agile board.

<h2>Getting started</h2>
To get started with this SDK, all you need to do is create a new Jira instance and choose how you want to target Jira:

'''C#
Jira jira = new Jira();
            jira.Connect("{{JIRA URL}}", "{{USERNAME}}", "{{PASSWORD}}");

            //Gets all of the projects configured in your jira instance
            List<Project> projects = jira.GetProjects();

            //Gets a specific project by name
            Project project = jira.GetProject("{{projectname}}");
            
            //Gets all of users favourite filters
            List<IssueFilter> filters = jira.GetFilters();

            //Gets a specific filter by name
            IssueFilter filter = jira.GetFilter("{{filtername}}");

            //Get a list of agile boards configured in your jira instance
            List<AgileBoard> agilaboards = jira.GetAgileBoards();

            //Get a specific issue with key
            Issue issue = jira.GetIssue("{{issuekey}}");
'''