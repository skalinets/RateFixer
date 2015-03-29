angular.module 'client', [ 'ngRoute','client-main','templates' ]
  
  .config ($routeProvider) ->

    $routeProvider
      .otherwise
        redirectTo: '/'