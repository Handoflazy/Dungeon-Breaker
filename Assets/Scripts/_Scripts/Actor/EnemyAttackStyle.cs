﻿using FirstVersion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAl
{
    public class EnemyMeleeAttack : EnemyAttack
    {
        public Animator animator;
        public float timeBetweenShots = 0.1f;
        public override void Attack(int damage)
        {
            if (waitBeforeNextAttack == false)
            {
                if (GetTarget().TryGetComponent<Damageable>(out Damageable body))
                {
                    body.DealDamage(damage, gameObject);
                }
                StartCoroutine(WaitBeforeAttackCoroutine());
            }
        }

        public override void RangeAttack(GameObject bulletPrefab, int numberOfBullets)
        {
            
            if (enemyAIBrain.Target.transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (enemyAIBrain.Target.transform.position.x < transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (waitBeforeNextAttack == false)
            {
                animator.SetTrigger("Attack");
                float maxScatterAngle = (numberOfBullets - 1) * 10;
                // Tính góc giữa các viên đạn
                float angleBetweenBullets = (2 * maxScatterAngle) / numberOfBullets;
                float initialScatterAngle = -maxScatterAngle;
                float bulletSpacing = 0f;
                for (int i = 0; i < numberOfBullets; i++)
                {
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    if (bullet.TryGetComponent<EnemyBullet>(out EnemyBullet bulletBullet))
                    {
                        bulletBullet.Damage = enemyAIBrain.statsData.damage;
                    }

                    Vector3 lookDirection = (enemyAIBrain.Target.transform.position - transform.position).normalized;
                    float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + initialScatterAngle;
                    bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.velocity = bullet.transform.right;
                    initialScatterAngle += angleBetweenBullets;
                    bullet.transform.position += bullet.transform.right * (bulletSpacing);
                    //Destroy(bullet, 3f);
                    Destroy(bullet, 3f);
                }

                StartCoroutine(WaitBeforeAttackCoroutine());
            }
        }

        public override void RangeAttackV2(GameObject bulletPrefab, int numberOfBullets)
        {
            
            if (enemyAIBrain.Target.transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (enemyAIBrain.Target.transform.position.x < transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Sửa góc quay khi bắn về phía trái
            }

            if (waitBeforeNextAttack == false)
            {
                StartCoroutine(FireBulletsContinuously(bulletPrefab, numberOfBullets));
                StartCoroutine(WaitBeforeAttackCoroutine());
            }
        }
        private IEnumerator FireBulletsContinuously(GameObject bulletPrefab, int numberOfBullets)
        {
            animator.SetTrigger("Attack");
            for (int i = 0; i < numberOfBullets; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                if (bullet.TryGetComponent<EnemyBullet>(out EnemyBullet bulletBullet))
                {
                    bulletBullet.Damage = enemyAIBrain.statsData.damage;
                }

                Vector3 lookDirection = (enemyAIBrain.Target.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.velocity = bullet.transform.right;

                Destroy(bullet, 3f); // Hủy viên đạn sau 3 giây

                yield return new WaitForSeconds(timeBetweenShots); // Đợi trước khi bắn viên đạn tiếp theo
            }
        }
    }
}