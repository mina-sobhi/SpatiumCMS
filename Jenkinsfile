pipeline {
    agent any
    stages {
        stage('Checkout') {
            steps {
                git 'https://github.com/mina-sobhi/SpatiumCMS'
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore'
            }
        }
        
        stage('Build') {
            steps {
            
                sh 'dotnet build'
            }
        }
        
    }
}