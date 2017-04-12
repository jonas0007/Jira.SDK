using Jira.SDK.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK.Tests
{
	public class MockJiraClient : IJiraClient
	{
		List<Project> _projects = new List<Project>()
		{
			new Project(){
				Key = "ITDEV",
				Name = "ITDev SCRUM",
				Lead = new User(){
					Key = "jverdick",
					Name = "Jonas Verdickt",
					DisplayName = "Jonas Verdickt",
					EmailAddress = "jonas.verdickt@staff.telenet.be"
				}
			},
			new Project(){
				Key = "QA",
				Name = "Quality Assurance SCRUM",
				Lead = new User(){
					Key = "vstaneva",
					Name = "Vanya Staneva",
					DisplayName = "Vanya Staneva",
					EmailAddress = "vanya.staneva@staff.telenet.be"
				}
			}
		};

        Dictionary<string, List<ProjectComponent>> _projectComponents = new Dictionary<string, List<ProjectComponent>>()
        {
            { "ITDEV", new List<ProjectComponent>() {
                new ProjectComponent() {
                    ID = 1,
                    Name = "Component1",
                    Description = "Description1"
                },
                new ProjectComponent() {
                    ID = 2,
                    Name = "Component2",
                    Description = "Description2"
                }
            }},

            { "QA", new List<ProjectComponent>() {
                new ProjectComponent() {
                    ID = 3,
                    Name = "Component3",
                    Description = "Description3"
                },
            }},
        };

		List<ProjectVersion> _projectVersions = new List<ProjectVersion>()
		{
			new ProjectVersion(){
				ID = 1,
				Name = "2014-01-01/2014-01-14",
				Project = new Project(){
					Key = "ITDEV",
					Name = "ITDev SCRUM",
					Lead = new User(){
						Key = "jverdick",
						Name = "Jonas Verdickt",
						DisplayName = "Jonas Verdickt",
						EmailAddress = "jonas.verdickt@staff.telenet.be"
					}
				},
				Released = true,
				Archived = true,
				StartDate = new DateTime(2014, 1, 1),
				ReleaseDate = new DateTime(2014, 1, 10),
				Description = "SPRINT JANUARI 1 2014 JANUARI 10 2014"
			},
			new ProjectVersion(){
				ID = 2,
				Name = "2014-01-10/2014-01-24",
				Project = new Project(){
					Key = "ITDEV",
					Name = "ITDev SCRUM",
					Lead = new User(){
						Key = "jverdick",
						Name = "Jonas Verdickt",
						DisplayName = "Jonas Verdickt",
						EmailAddress = "jonas.verdickt@staff.telenet.be"
					}
				},
				Released = false,
				Archived = false,
				StartDate = new DateTime(2014, 1, 10),
				ReleaseDate = DateTime.Now.AddDays(2),
				Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
			},
			new ProjectVersion(){
				ID = 1,
				Name = "2014-01-01/2014-01-14",
				Project = new Project(){
					Key = "QA",
					Name = "Quality Assurance SCRUM",
					Lead = new User(){
						Key = "vstaneva",
						Name = "Vanya Staneva",
						DisplayName = "Vanya Staneva",
						EmailAddress = "vanya.staneva@staff.telenet.be"
					}
				},
				Released = true,
				Archived = true,
				StartDate = new DateTime(2014, 1, 1),
				ReleaseDate = new DateTime(2014, 1, 10),
				Description = "SPRINT JANUARI 1 2014 JANUARI 10 2014"
			},
			new ProjectVersion(){
				ID = 2,
				Name = "2014-01-10/2014-01-24",
				Project = new Project(){
					Key = "QA",
					Name = "Quality Assurance SCRUM",
					Lead = new User(){
						Key = "vstaneva",
						Name = "Vanya Staneva",
						DisplayName = "Vanya Staneva",
						EmailAddress = "vanya.staneva@staff.telenet.be"
					}
				},
				Released = false,
				Archived = false,
				StartDate = new DateTime(2014, 1, 10),
				ReleaseDate = DateTime.Now.AddDays(2),
				Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
			}
		};

		private Dictionary<String, List<User>> _assignableUsersPerProject = new Dictionary<String, List<User>>(){
			{
				"ITDEV",
				new List<User>(){
					new User(){
						Key = "jverdick",
						Name = "Jonas Verdickt",
						DisplayName = "Jonas Verdickt",
						EmailAddress = "jonas.verdickt@staff.telenet.be"
					},
					new User(){
						Key = "mwillem",
						Name = "Marc Willems",
						DisplayName = "Marc Willems",
						EmailAddress = "marc.willems@staff.telenet.be"
					},
					new User(){
						Key = "sschelfh",
						Name = "Sam Schelfhout",
						DisplayName = "Sam Schelfhout",
						EmailAddress = "sam.schelfhout@staff.telenet.be"
					}
				}
			},
			{
				"QA",
				new List<User>(){
					new User(){
						Key = "vstaneva",
						Name = "Vanya Staneva",
						DisplayName = "Vanya Staneva",
						EmailAddress = "vanya.staneva@staff.telenet.be"
					},
					new User(){
						Key = "tmathe",
						Name = "Thabile Mathe",
						DisplayName = "Thabile Mathe",
						EmailAddress = "thabile.mathe@staff.telenet.be"
					},
					new User(){
						Key = "tvandoors",
						Name = "Thomas Vandoorselaere",
						DisplayName = "Thomas Vandoorselaere",
						EmailAddress = "thomas.vandoorselaere@staff.telenet.be"
					}
				}
			}
		};

		private List<AgileBoard> _agileBoards = new List<AgileBoard>(){
			new AgileBoard(){
				ID = 1,
				Name = "Development Scrum",
				SprintSupport = true
			},
			new AgileBoard(){
				ID = 2,
				Name = "DDC testing",
				SprintSupport = false
			},
			new AgileBoard(){
				ID = 3,
				Name = "QA Scrum",
				SprintSupport = true
			}
		};

		private List<Issue> _issues = new List<Issue>()
		{
			new Issue(){
				Key = "ITDEV-1",
				Fields = new IssueFields(){
					Summary = "Set up a new database",
					Description = "Set up a new database for a new project we want to launch",
					Assignee = new User(){
						Key = "mwillem",
						Name = "Marc Willems",
						DisplayName = "Marc Willems",
						EmailAddress = "marc.willems@staff.telenet.be"
					},
					Created = new DateTime(2013, 12, 1),
					FixVersions = new List<ProjectVersion>()
					{
						new ProjectVersion()
						{
							ID = 1,
							Name = "2014-01-01/2014-01-14",
							Project = new Project(){
								Key = "ITDEV",
								Name = "ITDev SCRUM",
								Lead = new User(){
									Key = "jverdick",
									Name = "Jonas Verdickt",
									DisplayName = "Jonas Verdickt",
									EmailAddress = "jonas.verdickt@staff.telenet.be"
								}
							},
							Released = true,
							Archived = true,
							StartDate = new DateTime(2014, 1, 1),
							ReleaseDate = new DateTime(2014, 1, 10),
							Description = "SPRINT JANUARI 1 2014 JANUARI 10 2014"
						}
					},
					Priority = new Priority()
					{
						ID = 1,
						Name = "Normal"
					},
					Parent = null,
					Project = new Project(){
						Key = "ITDEV",
						Name = "ITDev SCRUM",
						Lead = new User(){
							Key = "jverdick",
							Name = "Jonas Verdickt",
							DisplayName = "Jonas Verdickt",
							EmailAddress = "jonas.verdickt@staff.telenet.be"
						}
					},
					Reporter = new User(){
						Key = "jverdick",
						Name = "Jonas Verdickt",
						DisplayName = "Jonas Verdickt",
						EmailAddress = "jonas.verdickt@staff.telenet.be"
					},
					Resolution = new Resolution()
					{
						ID = 1,
						Name = "None",
						Description = "This issue isn't resolved yet"
					},
					Status = new Status(){
						ID = 1,
						Name = "Open"
					},
					TimeTracking = new TimeTracking()
					{
						OriginalEstimateSeconds = 1800,
						RemainingEstimateSeconds = 600,
						TimeSpentSeconds = 2000						
					},
					Updated = new DateTime(2014, 1, 1)
				}
			},
			new Issue(){
				Key = "ITDEV-2",
				Fields = new IssueFields(){
					Summary = "Deleting the old database",
					Description = "Deleting the old database for the old project we want to remove",
					Assignee = new User(){
						Key = "mwillem",
						Name = "Marc Willems",
						DisplayName = "Marc Willems",
						EmailAddress = "marc.willems@staff.telenet.be"
					},
					Created = new DateTime(2013, 12, 1),
					FixVersions = new List<ProjectVersion>()
					{
						new ProjectVersion()
						{
							ID = 1,
							Name = "2014-01-01/2014-01-14",
							Project = new Project(){
								Key = "ITDEV",
								Name = "ITDev SCRUM",
								Lead = new User(){
									Key = "jverdick",
									Name = "Jonas Verdickt",
									DisplayName = "Jonas Verdickt",
									EmailAddress = "jonas.verdickt@staff.telenet.be"
								}
							},
							Released = true,
							Archived = true,
							StartDate = new DateTime(2014, 1, 1),
							ReleaseDate = new DateTime(2014, 1, 10),
							Description = "SPRINT JANUARI 1 2014 JANUARI 10 2014"
						}
					},
					Priority = new Priority()
					{
						ID = 1,
						Name = "Normal"
					},
					Parent = null,
					Project = new Project(){
						Key = "ITDEV",
						Name = "ITDev SCRUM",
						Lead = new User(){
							Key = "jverdick",
							Name = "Jonas Verdickt",
							DisplayName = "Jonas Verdickt",
							EmailAddress = "jonas.verdickt@staff.telenet.be"
						}
					},
					Reporter = new User(){
						Key = "jverdick",
						Name = "Jonas Verdickt",
						DisplayName = "Jonas Verdickt",
						EmailAddress = "jonas.verdickt@staff.telenet.be"
					},
					Resolution = new Resolution()
					{
						ID = 1,
						Name = "None",
						Description = "This issue isn't resolved yet"
					},
					Status = new Status(){
						ID = 1,
						Name = "Open"
					},
					TimeTracking = new TimeTracking()
					{
						OriginalEstimateSeconds = 1800,
						RemainingEstimateSeconds = 600,
						TimeSpentSeconds = 2000						
					},
					Updated = new DateTime(2014, 1, 1)
				}
			},
			new Issue(){
				Key = "ITDEV-3",
				Fields = new IssueFields(){
					Summary = "Deleting the old database",
					Description = "Deleting the old database for the old project we want to remove",
					Assignee = new User(){
						Key = "mwillem",
						Name = "Marc Willems",
						DisplayName = "Marc Willems",
						EmailAddress = "marc.willems@staff.telenet.be"
					},
					Created = new DateTime(2013, 12, 1),
					FixVersions = new List<ProjectVersion>()
					{
						new ProjectVersion(){
							ID = 2,
							Name = "2014-01-10/2014-01-24",
							Project = new Project(){
								Key = "ITDEV",
								Name = "ITDev SCRUM",
								Lead = new User(){
									Key = "jverdick",
									Name = "Jonas Verdickt",
									DisplayName = "Jonas Verdickt",
									EmailAddress = "jonas.verdickt@staff.telenet.be"
								}
							},
							Released = false,
							Archived = false,
							StartDate = new DateTime(2014, 1, 10),
							ReleaseDate = DateTime.Now.AddDays(2),
							Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
						}
					},
					Priority = new Priority()
					{
						ID = 1,
						Name = "Normal"
					},
					Parent = null,
					Project = new Project(){
						Key = "ITDEV",
						Name = "ITDev SCRUM",
						Lead = new User(){
							Key = "jverdick",
							Name = "Jonas Verdickt",
							DisplayName = "Jonas Verdickt",
							EmailAddress = "jonas.verdickt@staff.telenet.be"
						}
					},
					Reporter = new User(){
						Key = "jverdick",
						Name = "Jonas Verdickt",
						DisplayName = "Jonas Verdickt",
						EmailAddress = "jonas.verdickt@staff.telenet.be"
					},
					Resolution = new Resolution()
					{
						ID = 1,
						Name = "None",
						Description = "This issue isn't resolved yet"
					},
					Status = new Status(){
						ID = 1,
						Name = "Open"
					},
					TimeTracking = new TimeTracking()
					{
						OriginalEstimateSeconds = 1800,
						RemainingEstimateSeconds = 600,
						TimeSpentSeconds = 2000						
					},
					Updated = new DateTime(2014, 1, 1)
				}
			},
			new Issue(){ // Third line support container
				Key = "ITDEV-4",
				Fields = new IssueFields(){
					Summary = "3rd line support",
					Description = "3rd line support",
					Assignee = null,
					Created = new DateTime(2014, 1, 10),
					FixVersions = new List<ProjectVersion>()
					{
						new ProjectVersion(){
							ID = 2,
							Name = "2014-01-10/2014-01-24",
							Project = new Project(){
								Key = "ITDEV",
								Name = "ITDev SCRUM",
								Lead = new User(){
									Key = "jverdick",
									Name = "Jonas Verdickt",
									DisplayName = "Jonas Verdickt",
									EmailAddress = "jonas.verdickt@staff.telenet.be"
								}
							},
							Released = false,
							Archived = false,
							StartDate = new DateTime(2014, 1, 10),
							ReleaseDate = DateTime.Now.AddDays(2),
							Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
						}
					},
					Priority = new Priority()
					{
						ID = 1,
						Name = "Normal"
					},
					Parent = null,
					Project = new Project(){
						Key = "ITDEV",
						Name = "ITDev SCRUM",
						Lead = new User(){
							Key = "jverdick",
							Name = "Jonas Verdickt",
							DisplayName = "Jonas Verdickt",
							EmailAddress = "jonas.verdickt@staff.telenet.be"
						}
					},
					Reporter = new User(){
						Key = "jverdick",
						Name = "Jonas Verdickt",
						DisplayName = "Jonas Verdickt",
						EmailAddress = "jonas.verdickt@staff.telenet.be"
					},
					Resolution = new Resolution()
					{
						ID = 1,
						Name = "None",
						Description = "This issue isn't resolved yet"
					},
					Status = new Status(){
						ID = 1,
						Name = "Open"
					},
					TimeTracking = new TimeTracking()
					{
						OriginalEstimateSeconds = 1800,
						RemainingEstimateSeconds = 600,
						TimeSpentSeconds = 2000						
					},
					Updated = new DateTime(2014, 1, 1),
					Subtasks = new List<Subtask>(){
						new Subtask(){
							Key = "ITDEV-5"
						},
						new Subtask(){
							Key = "ITDEV-6"
						},
						new Subtask(){
							Key = "ITDEV-7"
						}
					}
				}
			},
			new Issue(){ // The first 3rd line support issue, resolved the same day
				Key = "ITDEV-5",
				Fields = new IssueFields(){
					Summary = "Klant heeft problemen met Mail",
					Description = "Bla blabla blablaba blabl abla blabalbalb. Bla blabla blablaba blabl abla blabalbalb. Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.\nBla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.",
					Assignee = null,
					Created = new DateTime(2014, 1, 10),
					FixVersions = new List<ProjectVersion>()
					{
						new ProjectVersion(){
							ID = 2,
							Name = "2014-01-10/2014-01-24",
							Project = new Project(){
								Key = "ITDEV",
								Name = "ITDev SCRUM",
								Lead = new User(){
									Key = "jverdick",
									Name = "Jonas Verdickt",
									DisplayName = "Jonas Verdickt",
									EmailAddress = "jonas.verdickt@staff.telenet.be"
								}
							},
							Released = false,
							Archived = false,
							StartDate = new DateTime(2014, 1, 10),
							ReleaseDate = DateTime.Now.AddDays(2),
							Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
						}
					},
					Priority = new Priority()
					{
						ID = 1,
						Name = "Normal"
					},
					Parent = null,
					Project = new Project(){
						Key = "ITDEV",
						Name = "ITDev SCRUM",
						Lead = new User(){
							Key = "jverdick",
							Name = "Jonas Verdickt",
							DisplayName = "Jonas Verdickt",
							EmailAddress = "jonas.verdickt@staff.telenet.be"
						}
					},
					Reporter = new User(){
						Key = "jverdick",
						Name = "Jonas Verdickt",
						DisplayName = "Jonas Verdickt",
						EmailAddress = "jonas.verdickt@staff.telenet.be"
					},
					Resolution = new Resolution()
					{
						ID = 1,
						Name = "None",
						Description = "This issue isn't resolved yet"
					},
					Status = new Status(){
						ID = 1,
						Name = "Open"
					},
					TimeTracking = new TimeTracking()
					{
						OriginalEstimateSeconds = 1800,
						RemainingEstimateSeconds = 600,
						TimeSpentSeconds = 2000						
					},
					Updated = new DateTime(2014, 1, 10),
					ResolutionDate = new DateTime(2014, 1, 10)
				}
			},
			new Issue(){ // The second 3rd line support issue, resolved the day after
				Key = "ITDEV-6",
				Fields = new IssueFields(){
					Summary = "Klant heeft problemen met web",
					Description = "Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.",
					Assignee = null,
					Created = new DateTime(2014, 1, 10),
					FixVersions = new List<ProjectVersion>()
					{
						new ProjectVersion(){
							ID = 2,
							Name = "2014-01-10/2014-01-24",
							Project = new Project(){
								Key = "ITDEV",
								Name = "ITDev SCRUM",
								Lead = new User(){
									Key = "jverdick",
									Name = "Jonas Verdickt",
									DisplayName = "Jonas Verdickt",
									EmailAddress = "jonas.verdickt@staff.telenet.be"
								}
							},
							Released = false,
							Archived = false,
							StartDate = new DateTime(2014, 1, 10),
							ReleaseDate = DateTime.Now.AddDays(2),
							Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
						}
					},
					Priority = new Priority()
					{
						ID = 1,
						Name = "Normal"
					},
					Parent = null,
					Project = new Project(){
						Key = "ITDEV",
						Name = "ITDev SCRUM",
						Lead = new User(){
							Key = "jverdick",
							Name = "Jonas Verdickt",
							DisplayName = "Jonas Verdickt",
							EmailAddress = "jonas.verdickt@staff.telenet.be"
						}
					},
					Reporter = new User(){
						Key = "jverdick",
						Name = "Jonas Verdickt",
						DisplayName = "Jonas Verdickt",
						EmailAddress = "jonas.verdickt@staff.telenet.be"
					},
					Resolution = new Resolution()
					{
						ID = 1,
						Name = "None",
						Description = "This issue isn't resolved yet"
					},
					Status = new Status(){
						ID = 1,
						Name = "Open"
					},
					TimeTracking = new TimeTracking()
					{
						OriginalEstimateSeconds = 1800,
						RemainingEstimateSeconds = 600,
						TimeSpentSeconds = 2000						
					},
					Updated = new DateTime(2014, 1, 10),
					ResolutionDate = new DateTime(2014, 1, 10)
				}
			},
			new Issue(){ // The third 3rd line support issue, isn't resolved yet
				Key = "ITDEV-7",
				Fields = new IssueFields(){
					Summary = "Klant heeft problemen met zijn domein",
					Description = "Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.",
					Assignee = null,
					Created = new DateTime(2014, 1, 10),
					FixVersions = new List<ProjectVersion>()
					{
						new ProjectVersion(){
							ID = 2,
							Name = "2014-01-10/2014-01-24",
							Project = new Project(){
								Key = "ITDEV",
								Name = "ITDev SCRUM",
								Lead = new User(){
									Key = "jverdick",
									Name = "Jonas Verdickt",
									DisplayName = "Jonas Verdickt",
									EmailAddress = "jonas.verdickt@staff.telenet.be"
								}
							},
							Released = false,
							Archived = false,
							StartDate = new DateTime(2014, 1, 10),
							ReleaseDate = DateTime.Now.AddDays(2),
							Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
						}
					},
					Priority = new Priority()
					{
						ID = 1,
						Name = "Normal"
					},
					Parent = null,
					Project = new Project(){
						Key = "ITDEV",
						Name = "ITDev SCRUM",
						Lead = new User(){
							Key = "jverdick",
							Name = "Jonas Verdickt",
							DisplayName = "Jonas Verdickt",
							EmailAddress = "jonas.verdickt@staff.telenet.be"
						}
					},
					Reporter = new User(){
						Key = "jverdick",
						Name = "Jonas Verdickt",
						DisplayName = "Jonas Verdickt",
						EmailAddress = "jonas.verdickt@staff.telenet.be"
					},
					Resolution = new Resolution()
					{
						ID = 1,
						Name = "None",
						Description = "This issue isn't resolved yet"
					},
					Status = new Status(){
						ID = 1,
						Name = "Open"
					},
					TimeTracking = new TimeTracking()
					{
						OriginalEstimateSeconds = 1800,
						RemainingEstimateSeconds = 600,
						TimeSpentSeconds = 2000						
					},
					Updated = new DateTime(2014, 1, 10),
					ResolutionDate = DateTime.MinValue
				}
			}
		};

		private List<Worklog> _worklogs = new List<Worklog>()
		{
				new Worklog(){
					Author = new User(){
						Key = "jverdick",
						Name = "Jonas Verdickt",
						DisplayName = "Jonas Verdickt",
						EmailAddress = "jonas.verdickt@staff.telenet.be"
					},
					Comment = "Solved the ticket",
					Created = new DateTime(2014, 1, 2),
					Issue = new Issue(){ // The third 3rd line support issue, isn't resolved yet
					Key = "ITDEV-7",
					Fields = new IssueFields(){
						Summary = "Klant heeft problemen met zijn domein",
						Description = "Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.",
						Assignee = null,
						Created = new DateTime(2014, 1, 10),
						FixVersions = new List<ProjectVersion>()
						{
							new ProjectVersion(){
								ID = 2,
								Name = "2014-01-10/2014-01-24",
								Project = new Project(){
									Key = "ITDEV",
									Name = "ITDev SCRUM",
									Lead = new User(){
										Key = "jverdick",
										Name = "Jonas Verdickt",
										DisplayName = "Jonas Verdickt",
										EmailAddress = "jonas.verdickt@staff.telenet.be"
									}
								},
								Released = false,
								Archived = false,
								StartDate = new DateTime(2014, 1, 10),
								ReleaseDate = DateTime.Now.AddDays(2),
								Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
							}
						},
						Priority = new Priority()
						{
							ID = 1,
							Name = "Normal"
						},
						Parent = null,
						Project = new Project(){
							Key = "ITDEV",
							Name = "ITDev SCRUM",
							Lead = new User(){
								Key = "jverdick",
								Name = "Jonas Verdickt",
								DisplayName = "Jonas Verdickt",
								EmailAddress = "jonas.verdickt@staff.telenet.be"
							}
						},
						Reporter = new User(){
							Key = "jverdick",
							Name = "Jonas Verdickt",
							DisplayName = "Jonas Verdickt",
							EmailAddress = "jonas.verdickt@staff.telenet.be"
						},
						Resolution = new Resolution()
						{
							ID = 1,
							Name = "None",
							Description = "This issue isn't resolved yet"
						},
						Status = new Status(){
							ID = 1,
							Name = "Open"
						},
						TimeTracking = new TimeTracking()
						{
							OriginalEstimateSeconds = 1800,
							RemainingEstimateSeconds = 600,
							TimeSpentSeconds = 2000						
						},
						Updated = new DateTime(2014, 1, 10),
						ResolutionDate = DateTime.MinValue
					}
				}
			},
			new Worklog(){
				Author = new User(){
					Key = "mwillem",
					Name = "Marc Willems",
					DisplayName = "Marc Willems",
					EmailAddress = "marc.willems@staff.telenet.be"
				},
				Comment = "Solved the ticket",
				Created = new DateTime(2014, 1, 2),
				Issue = new Issue()
				{ // The third 3rd line support issue, isn't resolved yet
					Key = "ITDEV-7",
					Fields = new IssueFields()
					{
						Summary = "Klant heeft problemen met zijn domein",
						Description = "Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.",
						Assignee = null,
						Created = new DateTime(2014, 1, 10),
						FixVersions = new List<ProjectVersion>()
						{
							new ProjectVersion(){
								ID = 2,
								Name = "2014-01-10/2014-01-24",
								Project = new Project(){
									Key = "ITDEV",
									Name = "ITDev SCRUM",
									Lead = new User(){
										Key = "jverdick",
										Name = "Jonas Verdickt",
										DisplayName = "Jonas Verdickt",
										EmailAddress = "jonas.verdickt@staff.telenet.be"
									}
								},
								Released = false,
								Archived = false,
								StartDate = new DateTime(2014, 1, 10),
								ReleaseDate = DateTime.Now.AddDays(2),
								Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
							}
						},
						Priority = new Priority()
						{
							ID = 1,
							Name = "Normal"
						},
						Parent = null,
						Project = new Project(){
							Key = "ITDEV",
							Name = "ITDev SCRUM",
							Lead = new User(){
								Key = "jverdick",
								Name = "Jonas Verdickt",
								DisplayName = "Jonas Verdickt",
								EmailAddress = "jonas.verdickt@staff.telenet.be"
							}
						},
						Reporter = new User(){
							Key = "jverdick",
							Name = "Jonas Verdickt",
							DisplayName = "Jonas Verdickt",
							EmailAddress = "jonas.verdickt@staff.telenet.be"
						},
						Resolution = new Resolution()
						{
							ID = 1,
							Name = "None",
							Description = "This issue isn't resolved yet"
						},
						Status = new Status(){
							ID = 1,
							Name = "Open"
						},
						TimeTracking = new TimeTracking()
						{
							OriginalEstimateSeconds = 1800,
							RemainingEstimateSeconds = 600,
							TimeSpentSeconds = 2000						
						},
						Updated = new DateTime(2014, 1, 10),
						ResolutionDate = DateTime.MinValue
					}
				}
			},
			new Worklog(){
				Author = new User(){
					Key = "mwillem",
					Name = "Marc Willems",
					DisplayName = "Marc Willems",
					EmailAddress = "marc.willems@staff.telenet.be"
				},
				Comment = "Solved the ticket",
				Created = new DateTime(2014, 1, 2),
				Issue = new Issue()
				{ // The second 3rd line support issue, resolved the day after
					Key = "ITDEV-6",
					Fields = new IssueFields()
					{
						Summary = "Klant heeft problemen met web",
						Description = "Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.",
						Assignee = null,
						Created = new DateTime(2014, 1, 10),
						FixVersions = new List<ProjectVersion>()
						{
							new ProjectVersion(){
								ID = 2,
								Name = "2014-01-10/2014-01-24",
								Project = new Project(){
									Key = "ITDEV",
									Name = "ITDev SCRUM",
									Lead = new User(){
										Key = "jverdick",
										Name = "Jonas Verdickt",
										DisplayName = "Jonas Verdickt",
										EmailAddress = "jonas.verdickt@staff.telenet.be"
									}
								},
								Released = false,
								Archived = false,
								StartDate = new DateTime(2014, 1, 10),
								ReleaseDate = DateTime.Now.AddDays(2),
								Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
							}
						},
						Priority = new Priority()
						{
							ID = 1,
							Name = "Normal"
						},
						Parent = null,
						Project = new Project(){
							Key = "ITDEV",
							Name = "ITDev SCRUM",
							Lead = new User(){
								Key = "jverdick",
								Name = "Jonas Verdickt",
								DisplayName = "Jonas Verdickt",
								EmailAddress = "jonas.verdickt@staff.telenet.be"
							}
						},
						Reporter = new User(){
							Key = "jverdick",
							Name = "Jonas Verdickt",
							DisplayName = "Jonas Verdickt",
							EmailAddress = "jonas.verdickt@staff.telenet.be"
						},
						Resolution = new Resolution()
						{
							ID = 1,
							Name = "None",
							Description = "This issue isn't resolved yet"
						},
						Status = new Status(){
							ID = 1,
							Name = "Open"
						},
						TimeTracking = new TimeTracking()
						{
							OriginalEstimateSeconds = 1800,
							RemainingEstimateSeconds = 600,
							TimeSpentSeconds = 2000						
						},
						Updated = new DateTime(2014, 1, 10),
						ResolutionDate = new DateTime(2014, 1, 10)
					}
				}
			}
		};

		private Dictionary<Int32, List<Sprint>> _sprints = new Dictionary<int, List<Sprint>>(){
			{
				1,
				new List<Sprint>(){
					new Sprint(){
						ID = 1,
						Name = "2014-01-01/2014-01-14",
						State = "Closed"
					},
					new Sprint(){
						ID = 2,
						Name = "2014-01-14/2014-01-24",
						State = "Closed"
					},
					new Sprint(){
						ID = 3,
						Name = "2014-01-24/2014-02-",
						State = "Open"
					}
				}
			},
			{
				2,
				new List<Sprint>(){
					new Sprint(){
						ID = 4,
						Name = "2014-01-01/2014-01-14",
						State = "Closed"
					},
					new Sprint(){
						ID = 5,
						Name = "2014-01-14/2014-01-24",
						State = "Closed"
					},
					new Sprint(){
						ID = 6,
						Name = "2014-01-24/2014-02-",
						State = "Open"
					}
				}
			},
			{
				3,
				new List<Sprint>(){
					new Sprint(){
						ID = 7,
						Name = "2014-01-01/2014-01-14",
						State = "Closed"
					},
					new Sprint(){
						ID = 8,
						Name = "2014-01-14/2014-01-24",
						State = "Closed"
					},
					new Sprint(){
						ID = 9,
						Name = "2014-01-24/2014-02-",
						State = "Open"
					}
				}
			}
		};

		private Dictionary<Int32, List<Issue>> _issuesFromSprint = new Dictionary<int, List<Issue>>(){
			{
				1,
				new List<Issue>(){
					new Issue(){ // Third line support container
						Key = "ITDEV-4",
						Fields = new IssueFields(){
							Summary = "3rd line support",
							Description = "3rd line support",
							Assignee = null,
							Created = new DateTime(2014, 1, 10),
							FixVersions = new List<ProjectVersion>()
							{
								new ProjectVersion(){
									ID = 2,
									Name = "2014-01-10/2014-01-24",
									Project = new Project(){
										Key = "ITDEV",
										Name = "ITDev SCRUM",
										Lead = new User(){
											Key = "jverdick",
											Name = "Jonas Verdickt",
											DisplayName = "Jonas Verdickt",
											EmailAddress = "jonas.verdickt@staff.telenet.be"
										}
									},
									Released = false,
									Archived = false,
									StartDate = new DateTime(2014, 1, 10),
									ReleaseDate = DateTime.Now.AddDays(2),
									Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
								}
							},
							Priority = new Priority()
							{
								ID = 1,
								Name = "Normal"
							},
							Parent = null,
							Project = new Project(){
								Key = "ITDEV",
								Name = "ITDev SCRUM",
								Lead = new User(){
									Key = "jverdick",
									Name = "Jonas Verdickt",
									DisplayName = "Jonas Verdickt",
									EmailAddress = "jonas.verdickt@staff.telenet.be"
								}
							},
							Reporter = new User(){
								Key = "jverdick",
								Name = "Jonas Verdickt",
								DisplayName = "Jonas Verdickt",
								EmailAddress = "jonas.verdickt@staff.telenet.be"
							},
							Resolution = new Resolution()
							{
								ID = 1,
								Name = "None",
								Description = "This issue isn't resolved yet"
							},
							Status = new Status(){
								ID = 1,
								Name = "Open"
							},
							TimeTracking = new TimeTracking()
							{
								OriginalEstimateSeconds = 1800,
								RemainingEstimateSeconds = 600,
								TimeSpentSeconds = 2000						
							},
							Updated = new DateTime(2014, 1, 1),
							Subtasks = new List<Subtask>(){
								new Subtask(){
									Key = "ITDEV-5"
								},
								new Subtask(){
									Key = "ITDEV-6"
								},
								new Subtask(){
									Key = "ITDEV-7"
								}
							}
						}
					},
					new Issue(){ // The first 3rd line support issue, resolved the same day
						Key = "ITDEV-5",
						Fields = new IssueFields(){
							Summary = "Klant heeft problemen met Mail",
							Description = "Bla blabla blablaba blabl abla blabalbalb. Bla blabla blablaba blabl abla blabalbalb. Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.\nBla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.",
							Assignee = null,
							Created = new DateTime(2014, 1, 10),
							FixVersions = new List<ProjectVersion>()
							{
								new ProjectVersion(){
									ID = 2,
									Name = "2014-01-10/2014-01-24",
									Project = new Project(){
										Key = "ITDEV",
										Name = "ITDev SCRUM",
										Lead = new User(){
											Key = "jverdick",
											Name = "Jonas Verdickt",
											DisplayName = "Jonas Verdickt",
											EmailAddress = "jonas.verdickt@staff.telenet.be"
										}
									},
									Released = false,
									Archived = false,
									StartDate = new DateTime(2014, 1, 10),
									ReleaseDate = DateTime.Now.AddDays(2),
									Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
								}
							},
							Priority = new Priority()
							{
								ID = 1,
								Name = "Normal"
							},
							Parent = null,
							Project = new Project(){
								Key = "ITDEV",
								Name = "ITDev SCRUM",
								Lead = new User(){
									Key = "jverdick",
									Name = "Jonas Verdickt",
									DisplayName = "Jonas Verdickt",
									EmailAddress = "jonas.verdickt@staff.telenet.be"
								}
							},
							Reporter = new User(){
								Key = "jverdick",
								Name = "Jonas Verdickt",
								DisplayName = "Jonas Verdickt",
								EmailAddress = "jonas.verdickt@staff.telenet.be"
							},
							Resolution = new Resolution()
							{
								ID = 1,
								Name = "None",
								Description = "This issue isn't resolved yet"
							},
							Status = new Status(){
								ID = 1,
								Name = "Open"
							},
							TimeTracking = new TimeTracking()
							{
								OriginalEstimateSeconds = 1800,
								RemainingEstimateSeconds = 600,
								TimeSpentSeconds = 2000						
							},
							Updated = new DateTime(2014, 1, 10),
							ResolutionDate = new DateTime(2014, 1, 10)
						}
					},
					new Issue(){ // The second 3rd line support issue, resolved the day after
						Key = "ITDEV-6",
						Fields = new IssueFields(){
							Summary = "Klant heeft problemen met web",
							Description = "Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.",
							Assignee = null,
							Created = new DateTime(2014, 1, 10),
							FixVersions = new List<ProjectVersion>()
							{
								new ProjectVersion(){
									ID = 2,
									Name = "2014-01-10/2014-01-24",
									Project = new Project(){
										Key = "ITDEV",
										Name = "ITDev SCRUM",
										Lead = new User(){
											Key = "jverdick",
											Name = "Jonas Verdickt",
											DisplayName = "Jonas Verdickt",
											EmailAddress = "jonas.verdickt@staff.telenet.be"
										}
									},
									Released = false,
									Archived = false,
									StartDate = new DateTime(2014, 1, 10),
									ReleaseDate = DateTime.Now.AddDays(2),
									Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
								}
							},
							Priority = new Priority()
							{
								ID = 1,
								Name = "Normal"
							},
							Parent = null,
							Project = new Project(){
								Key = "ITDEV",
								Name = "ITDev SCRUM",
								Lead = new User(){
									Key = "jverdick",
									Name = "Jonas Verdickt",
									DisplayName = "Jonas Verdickt",
									EmailAddress = "jonas.verdickt@staff.telenet.be"
								}
							},
							Reporter = new User(){
								Key = "jverdick",
								Name = "Jonas Verdickt",
								DisplayName = "Jonas Verdickt",
								EmailAddress = "jonas.verdickt@staff.telenet.be"
							},
							Resolution = new Resolution()
							{
								ID = 1,
								Name = "None",
								Description = "This issue isn't resolved yet"
							},
							Status = new Status(){
								ID = 1,
								Name = "Open"
							},
							TimeTracking = new TimeTracking()
							{
								OriginalEstimateSeconds = 1800,
								RemainingEstimateSeconds = 600,
								TimeSpentSeconds = 2000						
							},
							Updated = new DateTime(2014, 1, 10),
							ResolutionDate = new DateTime(2014, 1, 10)
						}
					},
					new Issue(){ // The third 3rd line support issue, isn't resolved yet
						Key = "ITDEV-7",
						Fields = new IssueFields(){
							Summary = "Klant heeft problemen met zijn domein",
							Description = "Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.Bla blabla blablaba blabl abla blabalbalb.",
							Assignee = null,
							Created = new DateTime(2014, 1, 10),
							FixVersions = new List<ProjectVersion>()
							{
								new ProjectVersion(){
									ID = 2,
									Name = "2014-01-10/2014-01-24",
									Project = new Project(){
										Key = "ITDEV",
										Name = "ITDev SCRUM",
										Lead = new User(){
											Key = "jverdick",
											Name = "Jonas Verdickt",
											DisplayName = "Jonas Verdickt",
											EmailAddress = "jonas.verdickt@staff.telenet.be"
										}
									},
									Released = false,
									Archived = false,
									StartDate = new DateTime(2014, 1, 10),
									ReleaseDate = DateTime.Now.AddDays(2),
									Description = "SPRINT JANUARI 10 2014 JANUARI 24 2014"
								}
							},
							Priority = new Priority()
							{
								ID = 1,
								Name = "Normal"
							},
							Parent = null,
							Project = new Project(){
								Key = "ITDEV",
								Name = "ITDev SCRUM",
								Lead = new User(){
									Key = "jverdick",
									Name = "Jonas Verdickt",
									DisplayName = "Jonas Verdickt",
									EmailAddress = "jonas.verdickt@staff.telenet.be"
								}
							},
							Reporter = new User(){
								Key = "jverdick",
								Name = "Jonas Verdickt",
								DisplayName = "Jonas Verdickt",
								EmailAddress = "jonas.verdickt@staff.telenet.be"
							},
							Resolution = new Resolution()
							{
								ID = 1,
								Name = "None",
								Description = "This issue isn't resolved yet"
							},
							Status = new Status(){
								ID = 1,
								Name = "Open"
							},
							TimeTracking = new TimeTracking()
							{
								OriginalEstimateSeconds = 1800,
								RemainingEstimateSeconds = 600,
								TimeSpentSeconds = 2000						
							},
							Updated = new DateTime(2014, 1, 10),
							ResolutionDate = DateTime.MinValue
						}
					}
				}
			}
		};

		public List<Project> GetProjects()
		{
			return this._projects;
		}

		public Project GetProject(String projectKey)
		{
			return this._projects.Where(project => project.Key.Equals(projectKey)).FirstOrDefault();
		}

		public List<ProjectVersion> GetProjectVersions(String projectKey)
		{
			return this._projectVersions.Where(version => version.Project.Key.Equals(projectKey)).ToList();
		}

        public List<ProjectComponent> GetProjectComponents(String projectKey)
        {
            if (!_projectComponents.ContainsKey(projectKey))
            {
                return null;
            }

            return _projectComponents.First(x => x.Key.Equals(projectKey)).Value;
        }

		public User GetUser(String username)
		{
			return this._assignableUsersPerProject.SelectMany(project => project.Value).Where(user => user.Username.Equals(username)).FirstOrDefault();
		}

		public List<User> GetAssignableUsers(String projectKey)
		{
			return this._assignableUsersPerProject[projectKey];
		}

		public List<Domain.AgileBoard> GetAgileBoards()
		{
			return this._agileBoards;
		}

		public Issue GetIssue(String key)
		{
			return _issues.Where(issue => issue.Key.Equals(key)).FirstOrDefault();
		}

		public List<Issue> GetIssuesFromProjectVersion(String projectKey, String projectVersionName)
		{
			return _issues.Where(issue => issue.Fields.Project.Key.Equals(projectKey) && issue.Fields.FixVersions.Select(version => version.Name).Contains(projectVersionName)).ToList();
		}

		public WorklogSearchResult GetWorkLogs(String issueKey)
		{
			return new WorklogSearchResult()
			{
				Worklogs = _worklogs.Where(worklog => worklog.Issue.Key == issueKey).ToList()
			};
		}

		public List<Sprint> GetSprintsFromAgileBoard(int agileBoardID)
		{
			return _sprints[agileBoardID];
		}


		public List<Issue> GetIssuesFromSprint(int sprintID)
		{
			return _issuesFromSprint[sprintID];
		}


		public List<Field> GetFields()
		{
            return new List<Field>();
		}

		public List<Sprint> GetBacklogSprintsFromAgileBoard(int agileBoardID)
		{
            return new List<Sprint>();
		}

		public Sprint GetSprint(int agileBoardID, int sprintID)
		{
            return _sprints[agileBoardID].Where(sprint => sprint.ID == sprintID).FirstOrDefault();
		}

		public List<Issue> SearchIssues(string jql, int maxResults)
		{
            return new List<Issue>();
		}

		public List<Issue> GetSubtasksFromIssue(string issueKey)
		{
            return new List<Issue>();
        }

		public List<Issue> GetEpicIssuesFromProject(string projectName)
		{
            return new List<Issue>();
        }

		public Issue GetEpicIssueFromProject(string projectName, string epicName)
		{
            return _issues.First();
        }

		public List<Issue> GetIssuesWithEpicLink(string epicLink)
		{
            return new List<Issue>();
        }

		public List<IssueFilter> GetFavoriteFilters()
		{
            return new List<IssueFilter>();
		}


		public void AddIssue(Issue issue)
		{
			//
		}


        public Comment AddCommentToIssue(Issue issue, Comment comment)
        {
            throw new NotImplementedException();
        }

        public void TransitionIssue(Issue issue, Transition transition)
        {
            throw new NotImplementedException();
        }

        public List<Transition> GetTransitions(string issueKey)
        {
            throw new NotImplementedException();
        }


        public void TransitionIssue(Issue issue, Transition transition, Comment comment)
        {
            throw new NotImplementedException();
        }

        public Issue AddIssue(IssueFields fields)
        {
            throw new NotImplementedException();
        }

        public string GetBaseUrl()
        {
            return "http://jira.example.com/";
        }


        public bool CreateProject(CreateProject newProject)
        {
            return newProject != null && newProject.Key != "FAILED";
        }

        public bool UpdateProject(CreateProject updatedProject)
        {
            return updatedProject != null && !string.IsNullOrWhiteSpace(updatedProject.Key);
        }

        public ProjectCategory CreateProjectCategory(string Name, string Description)
        {
            return new ProjectCategory
            {
                Description = Description,
                Name = Name,
                Id = 10000,
            };
        }

        public List<ProjectCategory> GetProjectCategories()
        {
            return new List<ProjectCategory> {
                new ProjectCategory
                {
                    Id = 10100,
                    Description = "Test 1",
                    Name = "Test1",
                    Self = "SELF_URL",
                },
                new ProjectCategory
                {
                    Id = 10101,
                    Description = "Test 2",
                    Name = "Test2",
                    Self = "SELF_URL",
                }
            };
        }

        public List<ProjectType> GetProjectTypes()
        {
            return new List<ProjectType> {
                new ProjectType
                {
                    Color = "#C0C0C0",
                    Description = "Type 1",
                    Key = "type1",
                    FormattedKey = "Type1"
                },
                new ProjectType
                {
                    Color = "#C0C0C0",
                    Description = "Type 2",
                    Key = "type2",
                    FormattedKey = "Type2"
                }
            };
        }

        public List<ProjectRole> GetProjectRoles(string key)
        {
            return new List<ProjectRole>
            {
                new ProjectRole
                {
                    Name = "Administrators",
                    Id = 10001,
                    Self = "SELF_URL"
                }
            };
        }

        public ProjectRole AddGroupActor(string projectKey, int id, string group)
        {
            return new ProjectRole
                {
                    Name = "Administrators",
                    Id = 10001,
                    Self = "SELF_URL",
                    Actors = new List<ProjectRoleActor>
                    {
                    }
                };
        }

        public bool DeleteGroupActor(string projectKey, int id, string group)
        {
            return true;
        }

        public List<IssueSecurityScheme> GetIssueSecuritySchemes()
        {
            return new List<IssueSecurityScheme>();
        }

        public List<PermissionScheme> GetPermissionSchemes()
        {
            return new List<PermissionScheme>();
        }

        public List<NotificationScheme> GetNotificationSchemes()
        {
            return new List<NotificationScheme>();
        }

        public GroupResult GetGroup(string groupName)
        {
            return new GroupResult
            {
                Self = "GROUP_SELF_URL",
                MaxResults = 50,
                StartAt = 0,
                Total = 0,
                IsLast = true,
                Users = new List<User>()
            };
        }

        public void SetPriorityToIssue(Priority priority, Issue issueId)
        {
            // Implementing interface
            throw new NotImplementedException();
        }
    }
}
