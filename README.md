# WebChangeDetection
Desktop application witch notify you when some changes on requested webpage appear and send You notification on e-mail.
You can compare all website or specify element.
# Sample using
I'm waiting for promotions in the store. In the created program I can add a link to the product and if the price changes, I will receive a notification
# Technology
This app is full made in .NET WPF + C#. 
# How to use
1. Download repository
2. Change e-mail settings in WCD/Mail/MailSender.cs
3. Build and run 
4. Type url which you want to spy
5. Type id of element example: <br />
   if on website appear div id="products-header"/> type products-header<br />
   If You leave this field clear, you'll be comparing the entire page
6. Type how frequently do You want to check
7. Type Your e-mail address if you want to get notification<br />
    7.1 If You leave this field clear, emails will not be sent
8. Push detect button

Now You can minimalise or close main window. App will work in Tray.
When something on page is changed, windows notification should appear.
Also there is a history tab in the main window where you can find dates and results of checks.

Feel free to comment and put suggestion. 
