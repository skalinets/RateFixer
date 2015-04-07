angular.module('client', ['ngRoute', 'client-main', 'templates']).config(function($routeProvider) {
  return $routeProvider.otherwise({
    redirectTo: '/'
  });
});

'app controller goes here';


'common service goes here';


angular.module('client-main', ['ngRoute']).config(function($routeProvider) {
  return $routeProvider.when('/', {
    templateUrl: 'main/main.html',
    controller: 'MainCtrl'
  });
}).controller('MainCtrl', function($scope) {
  $scope.awesomeThings = ['HTML5 Boilerplate', 'AngularJS', 'Karma', 'Coffeescript', 'Less', 'Jade'];
  return $scope.newReport = function() {
    return $scope.awesomeThings = ['clicked', 'non-clicked'];
  };
});
