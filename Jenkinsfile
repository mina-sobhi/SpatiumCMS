pipeline {
    agent any
    stages {
        stage('Checkout') {
            steps {
                git 'https://github.com/mina-sobhi/SpatiumCMS/tree/Staging'
            }
        }
        stage('Build') {
            steps {
                sh 'dotnet build'
            }
        }
    }
}