pipeline {
    agent any
    stages {
        stage('Checkout') {
            steps {
                git 'https://github.com/mina-sobhi/SpatiumCMS'
            }
        }

        stage('Restore Dependencies') {
            steps {
                script {
                    sh 'dotnet restore '
                }
            }
        }
        
        stage('Build') {
            steps {
            
                sh 'dotnet build -c Release'
            }
        }
        
    }
}