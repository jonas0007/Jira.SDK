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
			throw new NotImplementedException();
		}

		public List<Sprint> GetBacklogSprintsFromAgileBoard(int agileBoardID)
		{
			throw new NotImplementedException();
		}

		public Sprint GetSprint(int agileBoardID, int sprintID)
		{
			throw new NotImplementedException();
		}

		public List<Issue> SearchIssues(string jql)
		{
			throw new NotImplementedException();
		}

		public List<Issue> GetSubtasksFromIssue(string issueKey)
		{
			throw new NotImplementedException();
		}

		public Dictionary<string, string> GetIssueCustomFieldsFromIssue(string key)
		{
			throw new NotImplementedException();
		}

		public List<Issue> GetEpicIssuesFromProject(string projectName)
		{
			throw new NotImplementedException();
		}

		public Issue GetEpicIssueFromProject(string projectName, string epicName)
		{
			throw new NotImplementedException();
		}

		public List<Issue> GetIssuesWithEpicLink(string epicLink)
		{
			throw new NotImplementedException();
		}

		public List<IssueFilter> GetFavoriteFilters()
		{
			throw new NotImplementedException();
		}


		public void AddIssue(Issue issue)
		{
			throw new NotImplementedException();
		}
	}
}
