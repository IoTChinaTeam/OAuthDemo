define(['oauth2Client'], function (client) {
    var $userDisplay = $(".app-user");
    var $signInButton = $(".app-login");
    var $signOutButton = $(".app-logout");

    client.setProvider('AAD', 'azureiotsolutions.onmicrosoft.com', '957cfb32-7907-4fb2-90a0-fafcf3afc56c');
    //client.setProvider('Google', '349887504252-4q03i53j0bvcu3uorokc39e1fcd2q1r7.apps.googleusercontent.com');

    client.handleCallback();

    $(".contentContainer").empty();

    var user = client.getUserName();
    if (user) {
        $userDisplay.html(user.userName);
        $userDisplay.show();
        $signInButton.hide();
        $signOutButton.show();

        client.getToken(function (token) {
            $.ajax({
                type: "GET",
                url: "/api/user/getdetails",
                headers: {
                    'Authorization': 'Bearer ' + token
                }
            }).done(function (data) {
                $(".contentContainer").append($("<p/>").text("Hello, " + user));
                $(".contentContainer").append("<br><p>properties:</p>");
                $.each(data, function (key, value) {
                    $(".contentContainer").append($("<p/>").text(key + " = " + value));
                });
            });
        });
    } else {
        $userDisplay.empty();
        $userDisplay.hide();
        $signInButton.show();
        $signOutButton.hide();

        $(".contentContainer").append("<p>Please login</p>");
    }

    $signOutButton.click(function () {
        client.logout();
    });
    $signInButton.click(function () {
        client.login();
    });
});