angular.module("shorts", [])
    .controller("UrlController", ["$scope", "$http", function ($scope, $http) {
        $scope.shortUrls = [
            { ShortUrlId: 1, Url: "http://google.com", Short: "/ERT123", Created: new Date(new Date().setHours(1)), Clicks: 0 },
            { ShortUrlId: 2, Url: "http://yandex.ru", Short: "/0fd13", Created: new Date(), Clicks: 0 },
        ]

        $scope.url = "http://microsoft.com";

        this.shortenUrl = function () {
            $http.post("/api/url/", JSON.stringify($scope.url))
                .then(function (response) {
                    $scope.shortened = response.data;
                },
                    function (response) { console.error("Posting URL failed"); }
                );
        };
    }]);