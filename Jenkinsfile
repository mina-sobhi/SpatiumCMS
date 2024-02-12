pipeline {
    agent any
    stages {
        stage('Checkout') {
            steps {
                git 'https://github.com/minaawad2030/Spatium-CMS.git'
            }
        }
        stage('Build') {
            steps {
                sh 'dotnet build'
            }
        }
    }
}