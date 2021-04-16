echo 'Stopping db...'
docker stop scorp_stack_db

echo 'Stopping backend...'
docker stop scorp_stack_backend

echo 'Stopping admin dashboard...'
docker stop scorp_stack_admin_dashboard