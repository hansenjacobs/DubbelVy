# DubbelVy
ASP.NET MVC application for completing audit of work performed, such as in a call center.

Technologies used:
ASP.NET
MVC 5
C#
SQL

Requirements:
As an admin, create/edit audit templates – assigning questions and point values.
As an admin, add new users to the system and select appropriate roles.
As an audit manager, edit existing audits completed by any auditor.
As an auditor, complete audit based on audit template.
As an auditor, enter comments on an audit.
As an auditor, edit existing audits completed by the auditor.
As a supervisor, view audits completed for their team.
As a supervisor, receive email if audit completed for one of their team members is below x score.
As an auditee, receive email when audit is completed on their work. Using an email API.
As an auditee, view history of audits.
As an auditee, submit a dispute for an audit.  The dispute workflow will:
  •	Route to Supervisor for approval
  •	Audit Manager for decision and correction if needed
  •	User & Supervisor will be notified dispute complete via email
As a/an Auditor/Manager/Supervisor, view dashboard of KPI’s – such as lowest performing auditee, overall AutoFail rate for period.
