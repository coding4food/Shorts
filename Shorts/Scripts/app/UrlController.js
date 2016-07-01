angular.module("shorts", ['ngRoute'])
    .config(['$locationProvider', '$routeProvider',
        function config($locationProvider, $routeProvider) {
            //$locationProvider.hashPrefix('!');

            $routeProvider.
              when('/', {
                  templateUrl: 'Scripts/app/home.html'
              }).
              when('/list', {
                  templateUrl: 'Scripts/app/link-list.html'
              }).
              otherwise('/');
        }
    ])
    .controller("UrlController", ["$scope", "$rootScope", "$http", function ($scope, $rootScope, $http) {
        $scope.url = { location: "http://microsoft.com" };

        $scope.shortenUrl = function () {
            $http.post("/api/url/", JSON.stringify($scope.url.location))
                .then(function (response) {
                    $scope.shortened = response.data;
                },
                    function (response) { console.error("Posting URL failed"); }
                );
        };

        $scope.getAllUrls = function () {
            $http.get("/api/url/")
                .then(function (response) {
                    console.dir(response.data);
                    $scope.shortUrls = response.data;
                },
                function (response) { console.error("Getting all URLs failed"); })
        };

        $rootScope.$on("$routeChangeStart", function (event, next, current) {
            if (next.originalPath === "/list") {
                $scope.getAllUrls();
            }
        });

    }]);