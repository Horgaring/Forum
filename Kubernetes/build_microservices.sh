if [ "$#" -lt 1 ]; then
    echo "Недостаточно аргументов"
    exit 1
fi
# Auth
docker build -t proxy021/forum1auth:$1  -f ./Services/Auth/Dockerfile ./
# apigetaway
docker build -t proxy021/forum1apigeteway:$1  -f ./ApiGateways/Api/Dockerfile ./
# Post
docker build -t proxy021/forum1post:$1  -f ./Services/Forum/Api/Dockerfile ./
# Comment
docker build -t proxy021/forum1comment:$1  -f ./Services/Comment/Api/Dockerfile ./
# Notification
docker build -t proxy021/forum1notification:$1  -f ./Services/Notification/Api/Dockerfile ./
# Identity
docker build -t proxy021/forum1identity:$1  -f ./Services/Identity/Api/Dockerfile ./
