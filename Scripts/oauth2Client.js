'use strict';

define([
    'https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js',
    'jso',
    'jwt_decode'], function (adal, jso, jwtDecode) {
    var _provider;
    var _aadContext;
    var _googleContext;

    function initializeAADContext(tenant, clientId) {
        _aadContext = new AuthenticationContext({
            instance: "https://login.microsoftonline.com/",
            tenant: tenant,
            clientId: clientId,
            postLogoutRedirectUri: window.location.origin,
            cacheLocation: "localStorage"
        });
    }

    function initializeGoogleContext(clientId) {
        _googleContext = new jso({
            providerID: "google",
            client_id: clientId,
            redirect_uri: window.location.origin,
            authorization: "https://accounts.google.com/o/oauth2/v2/auth",
            scopes: { request: ["openid", "profile", "email"] }
        });
    }

    return {
        setProvider: function (provider) {
            switch (provider) {
                case 'AAD':
                    if (arguments.length !== 3) {
                        throw "Invalidate parameters for AAD provider";
                    }

                    initializeAADContext(arguments[1], arguments[2]);
                    break;

                case 'Google':
                    if (arguments.length !== 2) {
                        throw "Invalidate parameters for Google provider";
                    }

                    initializeGoogleContext(arguments[1]);
                    break;

                default:
                    throw "Invalidate provider: " + provider;
            }

            _provider = provider;
        },

        getProvider: function () {
            return _provider;
        },

        handleCallback: function () {
            switch (_provider) {
                case 'AAD':
                    _aadContext.handleWindowCallback();
                    break;

                case 'Google':
                    _googleContext.callback();
                    break;

                default:
                    throw "Invalidate provider: " + provider;
            }
        },

        getUserName: function () {
            switch (_provider) {
                case 'AAD':
                    var aadUser = _aadContext.getCachedUser();
                    return aadUser ? aadUser.userName : null;

                case 'Google':
                    var googleToken = _googleContext.checkToken();
                    if (googleToken) {
                        var decodedGoogleToken = jwtDecode(googleToken.id_token);
                        return decodedGoogleToken.name;
                    } else {
                        return null;
                    }

                default:
                    throw "Invalidate provider: " + provider;
            }
        },

        getToken: function (callback) {
            switch (_provider) {
                case 'AAD':
                    _aadContext.acquireToken(_aadContext.config.clientId, function (error, token) {
                        callback(token);
                    });
                    break;

                case 'Google':
                    _googleContext.getToken(function (token) {
                        callback(token.id_token);
                    });
                    break;

                default:
                    throw "Invalidate provider: " + provider;
            }
        },

        login: function () {
            switch (_provider) {
                case 'AAD':
                    _aadContext.login();
                    break;

                case 'Google':
                    _googleContext.getToken();
                    break;

                default:
                    throw "Invalidate provider: " + provider;
            }
        },

        logout: function () {
            switch (_provider) {
                case 'AAD':
                    _aadContext.logOut();
                    break;

                case 'Google':
                    _googleContext.wipeTokens();
                    window.location = _googleContext.config.config.redirect_uri;
                    break;

                default:
                    throw "Invalidate provider: " + provider;
            }
        }
    };
});