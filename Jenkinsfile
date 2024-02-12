pipeline {
    agent any
    stages {
        stage('Checkout') {
            steps {
                git 'https://github.com/mina-sobhi/SpatiumCMS'
            }
        }
        stage('Build') {
            steps {
                sh 'dotnet build'
            }
        }
    }
}