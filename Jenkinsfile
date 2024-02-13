pipline{
    agent any

    stages{

        stage('Checkout'){
            git 'https://github.com/mina-sobhi/SpatiumCMS.git'
        }


        stage('Restore Dependencies') {
            steps {
                script {
                    sh 'dotnet restore'
                }
            }
        }
        
        stage('Build') {
            steps {
                script {
                    sh 'dotnet build -c Release'
                }
            }
        }




    }
}